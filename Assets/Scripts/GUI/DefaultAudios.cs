using UnityEngine;

public class DefaultAudios : MonoBehaviour
{
    private static DefaultAudios _instance;

    public AudioSource audioSourceFX;
    public AudioSource audioSourceMusic;

    public AudioClip buttonSound;

    public AudioClip menuSound;

    public AudioClip stageSelectSound;

    public AudioClip stage01Sound;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static DefaultAudios Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Start()
    {
        PlayMenuSound();
    }

    public void PlayStage01Sound()
    {
        audioSourceMusic.PlayOneShot(stage01Sound);
    }

    public void PlayStageSelectSound()
    {
        audioSourceMusic.PlayOneShot(stageSelectSound);
    }

    public void PlayMenuSound()
    {
        audioSourceMusic.PlayOneShot(menuSound);
    }

    public void PlayButton()
    {
        audioSourceFX.PlayOneShot(buttonSound);
    }
}
