using UnityEngine;

public class DialogueInteractable : InteractableObject
{
    [SerializeField] private DialogueSet dialogueSet;
    [SerializeField] private InspectableObject inspectableObject;


    private void Awake()
    {
        if (dialogueSet == null) throw new System.Exception("DialogueSet not set in " + name);
    }

    public override void Interact()
    {
        if (DialogueManager.Instance == null) return;

        DialogueManager.Instance.StartDialogue(dialogueSet);
        DialogueManager.Instance.OnDialogueEnd += OnDialogueEnd;
    }

    private void OnDialogueEnd()
    {
        DialogueManager.Instance.OnDialogueEnd -= OnDialogueEnd;
        if (inspectableObject != null) inspectableObject.Interact();
    }
}
