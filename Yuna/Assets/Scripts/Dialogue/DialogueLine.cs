using System;
using UnityEngine;

[Serializable]
public class DialogueLine
{
    [field: SerializeField] public string Speaker { get; private set; }
    [field: SerializeField] public string Text { get; private set; }
    // [field: SerializeField] public Sprite Portrait { get; private set; } // portrait (?)
}
