using TMPro;
using UnityEngine;

public class ChangeResolution : MonoBehaviour
{
    private TMP_Dropdown dropdown;

    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(); });
    }

    public void DropdownValueChanged()
    {
        switch (dropdown.value)
        {
            case 0:
                Settings.Instance.Resolution = Resolutions._1366x768;
                break;
            case 1:
                Settings.Instance.Resolution = Resolutions._1280x720;
                break;
            case 2:
                Settings.Instance.Resolution = Resolutions._1024x768;
                break;
            case 3:
                Settings.Instance.Resolution = Resolutions._800x600;
                break;
            case 4:
                Settings.Instance.Resolution = Resolutions._640x480;
                break;
        }
    }
}
