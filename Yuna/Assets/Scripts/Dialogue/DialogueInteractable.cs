using UnityEngine;

public class DialogueInteractable : InteractableObject
{
    [SerializeField] private DialogueSet dialogueSet;
    [SerializeField] private InspectableObject inspectableObject;


    private void Awake()
    {
        if (dialogueSet == null) throw new System.Exception($"DialogueSet not set in {name}");
        dialogueSet.InitDialogueSet();
    }

    private void Start()
    {
        if (DialogueManager.Instance == null || inspectableObject == null) return;
        if (!DialogueManager.Instance.HasSeenDialogue(dialogueSet.DialogueId)) return;

        // Interact with the inspectable object if the dialogue has already been seen
        inspectableObject.Interact();
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
