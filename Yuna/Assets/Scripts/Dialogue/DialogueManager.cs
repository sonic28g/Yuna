using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;
using TMPro;
using StarterAssets;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    private StarterAssetsInputs _inputs;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject _dialogueUI;
    [SerializeField] private TMP_Text _speakerText;
    [SerializeField] private TMP_Text _dialogueText;
    // [SerializeField] private Image _portraitImage; // portrait (?)

    [SerializeField] private float _textSpeed = 0.25f;

    private readonly HashSet<string> _seenDialogues = new();

    private readonly Queue<DialogueLine> _dialogueQueue = new();
    public bool IsDialogueActive { get; private set; } = false;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Disable the dialogue UI at the start
        if (_dialogueUI != null) _dialogueUI.SetActive(false);

        // Find StarterAssetsInputs 
        _inputs = FindFirstObjectByType<StarterAssetsInputs>();
        if (_inputs == null) return;

        // Subscribe to the SkipDialogue and NextLine actions
        _inputs.DialogueSkip += SkipDialogue;
        _inputs.DialogueNext += NextLine;
    }

    private void OnDestroy()
    {
        if (_inputs == null) return;

        // Unsubscribe from the SkipDialogue and NextLine actions
        _inputs.DialogueSkip -= SkipDialogue;
        _inputs.DialogueNext -= NextLine;
    }

    public bool HasSeenDialogue(string dialogueId) => _seenDialogues.Contains(dialogueId);


    public bool StartDialogue(DialogueSet dialogueSet)
    {
        if (IsDialogueActive) return false;
        if (dialogueSet == null || !dialogueSet.AreConditionsMet()) return false;

        // Pause input
        if (_inputs != null) _inputs.PauseInput(this);

        // Add the dialogue to the seen dialogues list
        IsDialogueActive = true;
        _seenDialogues.Add(dialogueSet.DialogueId);

        // Clear the dialogue queue and add the new dialogue lines
        _dialogueQueue.Clear();
        dialogueSet.GetLines().ForEach(line => _dialogueQueue.Enqueue(line));

        // Enable the dialogue UI and display the first (next) line
        if (_dialogueUI != null) _dialogueUI.SetActive(true);
        DisplayNextLine();

        return true;
    }

    private void EndDialogue()
    {
        // Disable the dialogue UI
        IsDialogueActive = false;
        if (_dialogueUI != null) _dialogueUI.SetActive(false);

        // Resume input
        if (_inputs != null) _inputs.ResumeInput(this);
    }


    public void SkipDialogue()
    {
        if (!IsDialogueActive) return;

        // Stop the TypeCurrentText coroutine and end the dialogue
        StopAllCoroutines();
        EndDialogue();
    }

    public void NextLine()
    {
        if (!IsDialogueActive) return;

        // Stop the TypeCurrentText coroutine and display the next line
        StopAllCoroutines();
        DisplayNextLine();
    }


    private void DisplayNextLine()
    {
        // If the dialogue queue is empty, end the dialogue
        if (_dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        // Get the next line from the dialogue queue
        DialogueLine line = _dialogueQueue.Dequeue();
        
        // Display the line's portrait (?), speaker and text (w/TypeCurrentText coroutine)
        // if (_portraitImage != null)
        // {
        //     _portraitImage.sprite = line.Portrait;
        //     _portraitImage.enabled = line.Portrait != null;
        // }
        if (_speakerText != null) _speakerText.text = line.Speaker;
        if (_dialogueText != null) StartCoroutine(TypeCurrentText(line.Text));
    }

    private IEnumerator TypeCurrentText(string currentText)
    {
        // Clear the dialogue text and type the current text letter by letter w/delay
        _dialogueText.text = "";
        foreach (char letter in currentText)
        {
            _dialogueText.text += letter;
            yield return new WaitForSeconds(_textSpeed);
        }
    }
}
