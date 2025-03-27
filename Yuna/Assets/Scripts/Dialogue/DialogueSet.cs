using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Yuna/Dialogue/DialogueSet")]
public class DialogueSet : ScriptableObject
{
    [field: SerializeField] public string DialogueId { get; private set; }
    [SerializeField] private List<DialogueLine> _lines;

    public List<DialogueLine> GetLines() => _lines.GetRange(0, _lines.Count);
}
