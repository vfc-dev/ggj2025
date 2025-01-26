using AudioSystem;
using UnityEngine;

public class GameMaster : Singleton<GameMaster>
{
    [Header("Sounds")]
    public SoundData cleanedSFX;

    [Header("Colors")]
    [ColorUsage(true, true)]
    public Color colorHighlight;


}
