using UnityEngine;
using UnityEngine.UI;

public class NavigateScreens : MonoBehaviour
{
    
    public GameObject stageSelect;
    public GameObject mainMenu;

    public void OpenStageSelectMenu()
    {
        stageSelect.SetActive(true);
        mainMenu.SetActive(false);
        DefaultAudios.Instance.PlayStageSelectSound();
    }

    public void OpenMainMenu()
    {
        stageSelect.SetActive(false);
        mainMenu.SetActive(true);
        DefaultAudios.Instance.PlayMenuSound();
    }
}
