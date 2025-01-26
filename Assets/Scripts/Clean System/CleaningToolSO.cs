using UnityEngine;

[CreateAssetMenu(fileName = "CleaningTool", menuName = "ScriptableObjects/NewCleaningTool", order = 0)]
public class CleaningToolSO : ScriptableObject
{
    public enum ToolType
    {
        Basic,
        Fire
    }
    public ToolType toolType;
    public float cleaningRadius = 0.5f; // The area cleaned per hit
    public Texture2D dirtBrush;
    public float distanceCheck = 5f;
}
