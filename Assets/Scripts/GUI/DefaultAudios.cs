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

    public AudioClip bubbleSpell;

    public AudioClip clearStage;

    public AudioClip clearObject;

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
        audioSourceMusic.Stop();
        audioSourceMusic.PlayOneShot(stage01Sound);
    }

    public void PlayStageSelectSound()
    {
        audioSourceMusic.Stop();
        audioSourceMusic.PlayOneShot(stageSelectSound);
    }

    public void PlayMenuSound()
    {
        audioSourceMusic.Stop();
        audioSourceMusic.PlayOneShot(menuSound);
    }

    public void PlayButton()
    {
        audioSourceFX.loop = false;
        audioSourceFX.PlayOneShot(buttonSound);
    }

    public void StartBubbleSpell()
    {
        if(audioSourceFX.clip == bubbleSpell && audioSourceFX.isPlaying)
        {
            return;
        }
        audioSourceFX.loop = true;
        audioSourceFX.clip = bubbleSpell;
        audioSourceFX.Play();
    }

    public void StopBubbleSpell()
    {
        audioSourceFX.Stop();
    }

    public void PlayClearStage()
    {
        audioSourceFX.Stop();
        audioSourceFX.loop = false;
        audioSourceFX.PlayOneShot(clearStage);
    }

    public void PlayClearObject()
    {
        audioSourceFX.Stop();
        audioSourceFX.loop = false;
        audioSourceFX.PlayOneShot(clearObject);
    }
}
