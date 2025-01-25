using System;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;


public class Settings : MonoBehaviour
{
    private static Settings _instance;
    [SerializeField]
    private Resolutions _resolution = Resolutions._1024x768;
    [SerializeField]
    private bool _windowed = true;
    [SerializeField]
    private float _brightness = 1.0f;
    [SerializeField]
    private float _masterVolume = 1.0f;
    [SerializeField]
    private float _musicVolume = 1.0f;
    [SerializeField]
    private float _sfxVolume = 1.0f;
    [SerializeField]
    private bool _muteMusic = false;
    [SerializeField]
    private bool _muteSFX = false;
    
    public AudioMixer audioMixer;


    public static Settings Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        if(_instance != null )
        {
            Destroy(gameObject);
            return;
        }
        Debug.Log(Screen.resolutions);
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool Windowed
    {
        get
        {
            return _windowed;
        }

        set
        {
            _windowed = value;
            Screen.fullScreen = !value;
            adjustScreen();
        }
    }

    public Resolutions Resolution
    {
        get
        {
            return _resolution;
        }

        set
        {
            _resolution = value;
            adjustScreen();
        }
    }

    public float Brightness
    {
        get
        {
            return _brightness;
        }

        set
        {
            _brightness = value;
            RenderSettings.ambientLight = new Color(_brightness, _brightness, _brightness);
        }
    }

    public float MasterVolume
    {
        get
        {
            return _masterVolume;
        }

        set
        {
            _masterVolume = value;
            audioMixer.SetFloat("Master", Mathf.Log10(_masterVolume) * 20);
        }
    }

    public float MusicVolume
    {
        get
        {
            return _musicVolume;
        }

        set
        {
            _musicVolume = value;
            adjustMusicVolume();
        }
    }

    public float SFXVolume
    {
        get
        {
            return _sfxVolume;
        }

        set
        {
            _sfxVolume = value;
            adjustSFXVolume();
        }
    }

    public bool MuteMusic
    {
        get
        {
            return _muteMusic;
        }

        set
        {
            _muteMusic = value;
            adjustMusicVolume();
        }
    }

    public bool MuteSFX
    {
        get
        {
            return _muteSFX;
        }

        set
        {
            _muteSFX = value;
            adjustSFXVolume();
        }
    }

    public void InvertMuteSfx()
    {
        MuteSFX = !MuteSFX;
    }

    public void InvertMuteMusic()
    {
        MuteMusic = !MuteMusic;
    }

    private void adjustScreen()
    {
        switch (_resolution)
        {
            case Resolutions._640x480:
                Screen.SetResolution(640, 480, _windowed ? FullScreenMode.Windowed : FullScreenMode.FullScreenWindow);
                break;
            case Resolutions._800x600:
                Screen.SetResolution(800, 600, _windowed ? FullScreenMode.Windowed : FullScreenMode.FullScreenWindow);
                break;
            case Resolutions._1024x768:
                Screen.SetResolution(1024, 768, _windowed ? FullScreenMode.Windowed : FullScreenMode.FullScreenWindow);
                break;
            case Resolutions._1280x720:
                Screen.SetResolution(1280, 720, _windowed ? FullScreenMode.Windowed : FullScreenMode.FullScreenWindow);
                break;
        }
    }

    private void adjustMusicVolume()
    {
        audioMixer.SetFloat("Music", _muteMusic ? -80 : Mathf.Log10(_musicVolume) * 20);
    }

    private void adjustSFXVolume()
    {
        audioMixer.SetFloat("SFX", _muteSFX ? -80 : Mathf.Log10(_sfxVolume) * 20);
    }
}
