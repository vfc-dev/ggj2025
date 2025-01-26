using UnityEngine;

public class LoadStage : MonoBehaviour
{
    public void Load(string stageName) {
        UnityEngine.SceneManagement.SceneManager.LoadScene(stageName);
    }
}
