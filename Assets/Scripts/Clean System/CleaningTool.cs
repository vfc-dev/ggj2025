using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CleaningTool : MonoBehaviour
{
    public LayerMask cleanableLayer;
    private Camera mainCam;
    [SerializeField] private ParticleSystem ps_Basic;
    public CleaningToolSO tool;
    private void Awake()
    {
        mainCam = Camera.main;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) // Left mouse button for cleaning
        {
            if(ps_Basic!=null)
            ps_Basic.Play();

            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, tool.distanceCheck, cleanableLayer))
            {
                CleanArea(hit);
                //Debug.Log($"Hitted {hit.transform.name}.");
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            if (ps_Basic != null)
                ps_Basic.Stop();
        }
    }

    void CleanArea(RaycastHit hit)
    {
        Vector2 textureCoord = hit.textureCoord;
        Collider[] dirtyAreas = Physics.OverlapSphere(hit.point, tool.cleaningRadius, cleanableLayer);
        foreach (Collider area in dirtyAreas)
        {
            // Trigger cleaning on the object
            area.GetComponent<Cleanable>()?.Clean(textureCoord, tool);
            area.GetComponentInChildren<Cleanable>()?.Clean(textureCoord, tool);
            //Debug.Log($"{area.GetComponent<Cleanable>().name} - {hit.point}");
        }

    }
    
}
