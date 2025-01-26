using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class StartStage01 : MonoBehaviour
{
    public int numberOfObjects;
    public GameObject winScreen;

    public GameObject player;

    void Start()
    {
        if (DefaultAudios.Instance != null)
        {
            DefaultAudios.Instance.PlayStage01Sound();
        }
    }


    public void clearObject()
    {
        numberOfObjects -= 1;

        if(numberOfObjects <= 0)
        {
            winScreen.SetActive(true);
            if(DefaultAudios.Instance != null)
            {
                DefaultAudios.Instance.PlayClearStage();
            }

            player.GetComponent<CleaningTool>().enabled = false;
            player.GetComponent<FirstPersonController>().enabled = false;
        }
    }

    public void OpenMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
