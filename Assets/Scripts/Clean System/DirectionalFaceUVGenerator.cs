using UnityEngine;
using System.Collections.Generic;

public class DirectionalFaceUVGenerator : MonoBehaviour
{
    public GameObject targetObject; // The object to analyze
    public Material quadMaterial;  // Material for the generated quad

    public enum FaceSide
    {
        Up,
        Down,
        Front,
        Back,
        Left,
        Right
    }

    public FaceSide selectedFace = FaceSide.Up; // Default direction is upward-facing

    [ContextMenu("Generate UV Quad")]
    public void GenerateUVQuad()
    {
        if (!targetObject)
        {
            Debug.LogError("Target object is not set.");
            return;
        }

        // Get the mesh from the target object
        MeshFilter targetMeshFilter = targetObject.GetComponent<MeshFilter>();
        if (!targetMeshFilter)
        {
            Debug.LogError("Target object has no MeshFilter.");
            return;
        }

        Mesh targetMesh = targetMeshFilter.sharedMesh;
        Vector3[] vertices = targetMesh.vertices;
        Vector3[] normals = targetMesh.normals;
        int[] triangles = targetMesh.triangles;

        // Transform to world space
        Transform targetTransform = targetObject.transform;

        // Get the reference direction for the selected face
        Vector3 referenceDirection = GetReferenceDirection(selectedFace);

        List<Vector3> filteredVertices = new List<Vector3>();

        // Analyze triangles to find the ones facing the selected direction
        for (int i = 0; i < triangles.Length; i += 3)
        {
            int index0 = triangles[i];
            Vector3 n0 = targetTransform.TransformDirection(normals[index0]);

            if (Vector3.Dot(n0, referenceDirection) > 0.5f) // Adjust the threshold if needed
            {
                // Add all triangle vertices if the face matches
                filteredVertices.Add(targetTransform.TransformPoint(vertices[triangles[i]]));
                filteredVertices.Add(targetTransform.TransformPoint(vertices[triangles[i + 1]]));
                filteredVertices.Add(targetTransform.TransformPoint(vertices[triangles[i + 2]]));
            }
        }

        if (filteredVertices.Count == 0)
        {
            Debug.LogWarning($"No faces found in the {selectedFace} direction.");
            return;
        }

        // Calculate bounds for the selected face
        Vector3 min = filteredVertices[0];
        Vector3 max = filteredVertices[0];

        foreach (var vertex in filteredVertices)
        {
            min = Vector3.Min(min, vertex);
            max = Vector3.Max(max, vertex);
        }

        Vector3 boundsCenter = (min + max) / 2;
        Vector3 boundsSize = max - min;

        // Get the width and height for the selected face
        Vector2 faceSize = GetFaceSize(boundsSize, selectedFace);

        // Generate the quad
        GameObject uvQuad = new GameObject($"UVQuad_{selectedFace}");
        uvQuad.transform.position = boundsCenter;
        uvQuad.transform.rotation = GetFaceRotation(selectedFace);

        MeshFilter quadMeshFilter = uvQuad.AddComponent<MeshFilter>();
        MeshRenderer quadRenderer = uvQuad.AddComponent<MeshRenderer>();
        quadRenderer.material = quadMaterial;

        Mesh quadMesh = new Mesh();
        quadMesh.vertices = new Vector3[]
        {
            new Vector3(-faceSize.x / 2, 0, -faceSize.y / 2),
            new Vector3(-faceSize.x / 2, 0, faceSize.y / 2),
            new Vector3(faceSize.x / 2, 0, faceSize.y / 2),
            new Vector3(faceSize.x / 2, 0, -faceSize.y / 2),
        };

        quadMesh.triangles = new int[]
        {
            0, 1, 2,
            0, 2, 3
        };

        quadMesh.uv = new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0)
        };

        quadMesh.RecalculateNormals();
        quadMeshFilter.mesh = quadMesh;

        uvQuad.transform.SetParent(targetObject.transform);
        uvQuad.AddComponent<Cleanable>();
        uvQuad.layer = targetObject.layer;
        uvQuad.AddComponent<MeshCollider>();
        uvQuad.GetComponent<MeshCollider>().sharedMesh = quadMesh;
    }

    // Get the reference direction for the chosen face
    private Vector3 GetReferenceDirection(FaceSide face)
    {
        return face switch
        {
            FaceSide.Up => Vector3.up,
            FaceSide.Down => Vector3.down,
            FaceSide.Front => Vector3.forward,
            FaceSide.Back => Vector3.back,
            FaceSide.Left => Vector3.left,
            FaceSide.Right => Vector3.right,
            _ => Vector3.up,
        };
    }

    // Get the proper rotation for the chosen face
    private Quaternion GetFaceRotation(FaceSide face)
    {
        return face switch
        {
            FaceSide.Up => Quaternion.Euler(0, 0, 0),
            FaceSide.Down => Quaternion.Euler(180, 0, 0),
            FaceSide.Front => Quaternion.Euler(90, 0, 0),
            FaceSide.Back => Quaternion.Euler(-90, 0, 0),
            FaceSide.Left => Quaternion.Euler(90, 0, 90),
            FaceSide.Right => Quaternion.Euler(90, 0, -90),
            _ => Quaternion.identity,
        };
    }

    // Get the width and height of the face based on its bounds
    private Vector2 GetFaceSize(Vector3 boundsSize, FaceSide face)
    {
        return face switch
        {
            FaceSide.Up or FaceSide.Down => new Vector2(boundsSize.x, boundsSize.z),
            FaceSide.Front or FaceSide.Back => new Vector2(boundsSize.x, boundsSize.y),
            FaceSide.Left or FaceSide.Right => new Vector2(boundsSize.z, boundsSize.y),
            _ => Vector2.zero,
        };
    }
}
