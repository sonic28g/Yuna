using UnityEngine;

public class DialogueInteractable : InteractableObject
{
    [SerializeField] private DialogueSet dialogueSet;
    [SerializeField] private InspectableObject inspectableObject;
    [SerializeField] private GameObject talkIndicator;

    private void Awake()
    {
        if (dialogueSet == null) throw new System.Exception($"DialogueSet not set in {name}");
        dialogueSet.InitDialogueSet();
    }

    private void Start()
    {
        if (DialogueManager.Instance == null) return;
        if (!DialogueManager.Instance.HasSeenDialogue(dialogueSet.DialogueId)) return;

        if (talkIndicator != null) Destroy(talkIndicator);
        
        if (!dialogueSet.AreConditionsMet()) Destroy(this);
    }

    public override void Interact()
    {
        if (DialogueManager.Instance == null) return;

        bool started = DialogueManager.Instance.StartDialogue(dialogueSet);
        if (!started) return;

        if (talkIndicator != null) Destroy(talkIndicator);
        DialogueManager.Instance.OnDialogueEnd += OnDialogueEnd;
    }

    private void OnDialogueEnd()
    {
        DialogueManager.Instance.OnDialogueEnd -= OnDialogueEnd;

        if (inspectableObject != null) inspectableObject.Interact();
        if (!dialogueSet.AreConditionsMet()) Destroy(this);
    }
}
