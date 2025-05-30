using UnityEngine;

public enum InspectableType
{
    Evidence,
    Clue,
    Lore
}

[CreateAssetMenu(fileName = "New Inspectable", menuName = "Yuna/Inspectable")]
public class InspectableData : ScriptableObject
{
    public string inspectableID;
    public InspectableType type;
    public string inspectableTitle;
    [TextArea] public string inspectableDescription;

    public bool isFound = false;
    public bool isActive = true;
    
}