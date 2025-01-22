using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MagicCleaner : MonoBehaviour
{
    private Camera mainCam;
    [SerializeField] private LayerMask CanBeHit;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100, CanBeHit))
            {
                DecalProjector decal = hit.collider.GetComponent<DecalProjector>();
                if (decal != null)
                {
                    decal.fadeFactor -= 0.05f;
                    Debug.Log($"{decal.gameObject.name} - {decal.fadeFactor}");
                    if(decal.fadeFactor <= 0.05f)
                    {
                        Destroy(decal.gameObject);
                        Debug.Log($"{decal.gameObject.name} - Destroyed");
                    }
                }
            }
        }
    }
}
