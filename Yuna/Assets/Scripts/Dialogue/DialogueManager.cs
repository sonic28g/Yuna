using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;
using TMPro;
using StarterAssets;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    public Action OnDialogueEnd;

    private StarterAssetsInputs _inputs;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject _dialogueUI;
    [SerializeField] private TMP_Text _speakerText;
    [SerializeField] private TMP_Text _dialogueText;
    [SerializeField] private TMP_Text _briefingText;

    // [SerializeField] private Image _portraitImage; // portrait (?)

    [SerializeField] private float _textSpeed = 0.25f;
    private string _currentText = "";
    private bool _isTyping = false;

    private readonly HashSet<string> _seenDialogues = new();

    private readonly Queue<DialogueLine> _dialogueQueue = new();
    public bool IsDialogueActive { get; private set; } = false;
    private bool _isSkippable = true;


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
        _inputs.DialogueNext += NextLineOrFinishCurrent;
    }

    private void Start() => DialogueSet.LoadAllDialogueSets();

    private void OnDestroy()
    {
        if (_inputs == null) return;

        // Unsubscribe from the SkipDialogue and NextLine actions
        _inputs.DialogueSkip -= SkipDialogue;
        _inputs.DialogueNext -= NextLineOrFinishCurrent;
    }

    
    public bool HasSeenDialogue(string dialogueId) => _seenDialogues.Contains(dialogueId);

    public void SetDialogueAsSeen(string dialogueId)
    {
        if (string.IsNullOrEmpty(dialogueId)) return;
        if (HasSeenDialogue(dialogueId)) return;

        _seenDialogues.Add(dialogueId);
    }


    public bool StartDialogue(DialogueSet dialogueSet)
    {
        if (IsDialogueActive) return false;
        if (dialogueSet == null || !dialogueSet.AreConditionsMet()) return false;

        IsDialogueActive = true;
        _isSkippable = dialogueSet.Skippable;

        // Pause input
        if (_inputs != null) _inputs.PauseInput(this);

        // Clear the dialogue queue and add the new dialogue lines
        _dialogueQueue.Clear();
        dialogueSet.GetLines().ForEach(line =>
        {
            _dialogueQueue.Enqueue(line);
            _briefingText.text += line.Text + "\n";
        });

        // Add the dialogue to the seen dialogues list
        SetDialogueAsSeen(dialogueSet.DialogueId);

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

        // Invoke the OnDialogueEnd action
        OnDialogueEnd?.Invoke();
    }


    public void SkipDialogue()
    {
        if (!IsDialogueActive) return;

        // If the dialogue is not skippable, just display the next line
        if (!_isSkippable) NextLineOrFinishCurrent();
        else
        {
            // End the dialogue
            FinishCurrentText();
            EndDialogue();
        }
    }

    public void NextLineOrFinishCurrent()
    {
        if (!IsDialogueActive) return;
        
        // Display the current or next line
        if (_isTyping) FinishCurrentText();
        else DisplayNextLine();
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
        if (_dialogueText != null)
        {
            _currentText = line.Text;
            StartCoroutine(TypeCurrentText());
        }
    }

    private IEnumerator TypeCurrentText()
    {
        _isTyping = true;

        // Clear the dialogue text and type the current text letter by letter w/delay
        _dialogueText.text = "";
        foreach (char letter in _currentText)
        {
            _dialogueText.text += letter;
            yield return new WaitForSeconds(_textSpeed);
        }

        _isTyping = false;
    }

    private void FinishCurrentText()
    {
        if (!_isTyping) return;

        // Stop the TypeCurrentText coroutine
        StopAllCoroutines();
        _isTyping = false;

        // Set the dialogue text to the current text immediately
        _dialogueText.text = _currentText; 
    }
}
