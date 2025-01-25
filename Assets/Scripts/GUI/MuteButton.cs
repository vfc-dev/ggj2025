using UnityEngine;
using UnityEngine.UI;

public enum AudioType
{
    Music,
    SFX
}

public class MuteButton : MonoBehaviour
{
    public Image image;

    public Sprite enabledIcon;
    public Sprite disabledIcon;

    public AudioType audioType = AudioType.Music;

    

    void Update()
    {
        if(audioType == AudioType.Music)
        {
            if(Settings.Instance.MuteMusic)
            {
                image.sprite = disabledIcon;
            }
            else
            {
                image.sprite = enabledIcon;
            }
        }
        else
        {
            if(Settings.Instance.MuteSFX)
            {
                image.sprite = disabledIcon;
            }
            else
            {
                image.sprite = enabledIcon;
            }
        }
    }
}
