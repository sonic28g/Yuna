using System;
using UnityEngine;

[Serializable]
public class DialogueCondition
{
    [SerializeField] private DialogueConditionType _conditionType;
    [SerializeField] private string _value;
    [SerializeField] private bool _negated;


    public bool Evaluate()
    {
        bool result = _conditionType switch
        {
            DialogueConditionType.SeenDialogue => DialogueManager.Instance != null && DialogueManager.Instance.HasSeenDialogue(_value),
            _ => throw new ArgumentOutOfRangeException()
        };

        return _negated ? !result : result;
    }


    public enum DialogueConditionType
    {
        SeenDialogue,
    }
}