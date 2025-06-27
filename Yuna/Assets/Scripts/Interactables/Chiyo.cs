using UnityEngine;
using UnityEngine.InputSystem;

public class Chiyo : MonoBehaviour
{
    [SerializeField] string dialogueID;
    [SerializeField] GameObject tessenPanel;
    [SerializeField] string helperTitle;
    [SerializeField] Sprite helperImage;
    [SerializeField] string helperText;
    private bool alreadyCompleted = false;

    private void Start()
    {
        if (DialogueManager.Instance != null && DialogueManager.Instance.HasSeenDialogue(dialogueID))
        {
            alreadyCompleted = true;
            WeaponSwitcher.instance.canSwitchWeapons = true;
            tessenPanel.SetActive(true);
        }
    }

    void Update()
    {
        if (!alreadyCompleted && DialogueManager.Instance != null && DialogueManager.Instance.HasSeenDialogue(dialogueID))
        {
            alreadyCompleted = true; // marca que já tratou disto

            WeaponSwitcher.instance.canSwitchWeapons = true;
            tessenPanel.SetActive(true);
            TutorialManager.Instance.MarkCompleted();

            var playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
            var attackBinding = playerInput.actions["Shoot"].bindings[0]; 
            string attackKey = InputControlPath.ToHumanReadableString(
                attackBinding.effectivePath, 
                InputControlPath.HumanReadableStringOptions.OmitDevice);

            helperText = $"Yuna can now switch weapons by pressing key 1 or 2. Press {attackKey} to use the Tessen. The tessen allows Yuna to attack enemies in the back.";

            UIManager.instance.ShowHelper(helperTitle, helperText, helperImage);
        }
    }
}
