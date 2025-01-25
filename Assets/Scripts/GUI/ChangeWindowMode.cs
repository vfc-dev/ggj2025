using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeWindowMode : MonoBehaviour
{
    private TMP_Dropdown dropdown;

    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(); });
    }

    public void DropdownValueChanged()
    {
        if (dropdown.value == 0)
        {
            Settings.Instance.Windowed = true;
        } else
        {
            Settings.Instance.Windowed = false;
        }
    }
}
