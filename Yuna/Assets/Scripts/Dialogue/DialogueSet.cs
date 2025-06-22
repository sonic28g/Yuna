using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Yuna/Dialogue/DialogueSet")]
public class DialogueSet : ScriptableObject
{
    [field: SerializeField] public string DialogueId { get; private set; }
    [field: SerializeField] public bool Skippable { get; private set; } = true;


    [SerializeField] private List<DialogueLine> _lines;
    [SerializeField] private List<DialogueCondition> _conditions = new();

    public List<DialogueLine> GetLines() => _lines.GetRange(0, _lines.Count);
    public bool AreConditionsMet() => _conditions.TrueForAll(c => c.Evaluate());

}
