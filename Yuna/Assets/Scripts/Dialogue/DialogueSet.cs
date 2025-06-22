using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(menuName = "Yuna/Dialogue/DialogueSet")]
public class DialogueSet : ScriptableObject
{
    private string _dialogueSetDir;
    private string DialogueSetFilePath => Path.Combine(_dialogueSetDir, $"{DialogueId}.json");
    private DialogueData _dialogueData;

    private static Action _loadAllDialogueSets;
    private static Action _saveAllDialogueSets;

    public static void LoadAllDialogueSets() => _loadAllDialogueSets?.Invoke();
    public static void SaveAllDialogueSets() => _saveAllDialogueSets?.Invoke();


    [field: SerializeField] public string DialogueId { get; private set; }
    [field: SerializeField] public bool Skippable { get; private set; } = true;

    [SerializeField] private List<DialogueLine> _lines;
    [SerializeField] private List<DialogueCondition> _conditions = new();


    public List<DialogueLine> GetLines() => _lines.GetRange(0, _lines.Count);
    public bool AreConditionsMet() => _conditions.TrueForAll(c => c.Evaluate());


    public void InitDialogueSet()
    {
        _dialogueSetDir = Path.Combine(Application.persistentDataPath, "Dialogue");
        LoadDialogueSet();

        _loadAllDialogueSets += LoadDialogueSet;
        _saveAllDialogueSets += SaveDialogueSet;
    }
    
    private void OnDestroy()
    {
        if (_dialogueSetDir == null) return;
        _loadAllDialogueSets -= LoadDialogueSet;
        _saveAllDialogueSets -= SaveDialogueSet;
    }


    private void LoadDialogueSet()
    {
        if (_dialogueSetDir == null) return;

        // Already loaded
        if (_dialogueData != null)
        {
            if (_dialogueData.Seen && DialogueManager.Instance != null) DialogueManager.Instance.SetDialogueAsSeen(DialogueId);
            return;
        }

        try
        {
            // Read the JSON file and deserialize it + Set the data variable
            string json = File.ReadAllText(DialogueSetFilePath);
            _dialogueData = JsonUtility.FromJson<DialogueData>(json) ?? throw new Exception($"Failed to parse dialogue data from {DialogueSetFilePath}");
        }
        catch (Exception e)
        {
            Debug.Log($"Failed to load dialogue set data for {name}: {e.Message}");

            // Initialize with default values if load fails
            _dialogueData = new DialogueData();
        }
    }

    public void SaveDialogueSet()
    {
        // Only if has been seen
        if (DialogueManager.Instance == null) return;
        if (!DialogueManager.Instance.HasSeenDialogue(DialogueId)) return;

        // Save data variable
        _dialogueData ??= new DialogueData();
        _dialogueData.Seen = true;

        try
        {
            // Convert the data to JSON
            string json = JsonUtility.ToJson(_dialogueData);

            // Create the directory if it doesn't exist
            if (!Directory.Exists(_dialogueSetDir)) Directory.CreateDirectory(_dialogueSetDir);

            // Save the JSON to a file
            File.WriteAllText(DialogueSetFilePath, json);
        }
        catch (Exception e)
        {
            Debug.Log($"Failed to save dialogue set data for {name}: {e.Message}");
        }
    }


    [Serializable]
    private class DialogueData
    {
        public bool Seen = false;
    }
}
