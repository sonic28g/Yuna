using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// TODO: UI
// using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    // TODO: UI
    /*
    [SerializeField] private GameObject _dialogueUI;
    [SerializeField] private Text _nameText;
    [SerializeField] private Text _dialogueText;
    */
    [SerializeField] private float _textSpeed = 0.25f;

    private readonly HashSet<string> _seenDialogues = new();

    private readonly Queue<DialogueLine> _dialogueQueue = new();
    public bool IsDialogueActive { get; private set; } = false;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool HasSeenDialogue(string dialogueId) => _seenDialogues.Contains(dialogueId);


    public void StartDialogue(DialogueSet dialogueSet)
    {
        if (IsDialogueActive) return;

        IsDialogueActive = true;
        _seenDialogues.Add(dialogueSet.DialogueId);

        _dialogueQueue.Clear();
        dialogueSet.GetLines().ForEach(line => _dialogueQueue.Enqueue(line));

        // TODO: UI
        // _dialogueUI.SetActive(true);
        Debug.Log("DialogueUI should be active");

        DisplayNextLine();
    }

    private void EndDialogue()
    {
        IsDialogueActive = false;
        // TODO: UI
        // _dialogueUI.SetActive(false);
        Debug.Log("DialogueUI should be inactive");
    }


    private void Update()
    {
        if (!IsDialogueActive) return;

        // TODO: New Input System
        if (Input.GetKeyDown(KeyCode.S)) SkipDialogue();
        else if (Input.GetKeyDown(KeyCode.Space)) NextLine();
    }

    public void SkipDialogue()
    {
        StopAllCoroutines();
        EndDialogue();
    }

    public void NextLine()
    {
        StopAllCoroutines();
        DisplayNextLine();
    }


    private void DisplayNextLine()
    {
        if (_dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = _dialogueQueue.Dequeue();

        // TODO: UI
        // _nameText.text = line.Speaker;
        Debug.Log(line.Speaker + ": " + line.Text);

        StartCoroutine(TypeCurrentText(line.Text));
    }

    private IEnumerator TypeCurrentText(string currentText)
    {
        // TODO: UI
        // _dialogueText.text = "";
        string txt = "";

        foreach (char letter in currentText)
        {
            // TODO: UI
            // _dialogueText.text += letter;
            txt += letter;
            Debug.Log(txt);

            yield return new WaitForSeconds(_textSpeed);
        }
    }
}
