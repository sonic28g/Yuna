using System;
using UnityEngine;

[Serializable]
public class DialogueLine
{
    [field: SerializeField] public string Speaker { get; private set; }
    [field: SerializeField] public string Text { get; private set; }
}
