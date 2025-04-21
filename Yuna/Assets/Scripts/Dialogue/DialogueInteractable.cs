using UnityEngine;

public class DialogueInteractable : InteractableObject
{
    [SerializeField] private DialogueSet dialogueSet;


    private void Awake()
    {
        if (dialogueSet == null) throw new System.Exception("DialogueSet not set in " + name);
    }

    public override void Interact()
    {
        if (DialogueManager.Instance != null) DialogueManager.Instance.StartDialogue(dialogueSet);
    }
}
