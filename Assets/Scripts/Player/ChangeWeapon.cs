using UnityEngine;

public class ChangeWeapon : MonoBehaviour
{
    public GameObject clearStaff;
    public GameObject fireStaff;

    public CleaningToolSO clear;
    public CleaningToolSO fire;

    public ParticleSystem psClear;
    public ParticleSystem psFire;

    private bool isClear = true;

    private CleaningTool cleaningTool;

    private void Start()
    {
        cleaningTool = GetComponent<CleaningTool>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (isClear)
            {
                cleaningTool.tool = fire;
                cleaningTool.ps_Basic = psFire;
                clearStaff.SetActive(false);
                fireStaff.SetActive(true);
                isClear = false;
            } else
            {
                cleaningTool.tool = clear;
                cleaningTool.ps_Basic = psClear;
                clearStaff.SetActive(true);
                fireStaff.SetActive(false);
                isClear = true;
            }
        }
    }
}
