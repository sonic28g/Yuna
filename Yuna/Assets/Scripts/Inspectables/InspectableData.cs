using System;
using System.IO;
using UnityEngine;

public enum InspectableType
{
    Evidence,
    Clue,
    Lore
}

[CreateAssetMenu(fileName = "New Inspectable", menuName = "Yuna/Inspectable")]
public class InspectableData : ScriptableObject
{
    public string inspectableID;
    public InspectableType type;
    public string inspectableTitle;
    [TextArea] public string inspectableDescription;

    public bool isFound = false;
    public bool isActive = true;

    private string _inspectableDir;
    private string InspectableFilePath => Path.Combine(_inspectableDir, $"{inspectableID}.json");
    private InspecData _inspecData;

    private static Action _saveAllInspectables;
    public static void SaveAllInspectables() => _saveAllInspectables?.Invoke();


    public void InitInspectable()
    {
        _inspectableDir = Path.Combine(Application.persistentDataPath, "Inspectable");
        LoadInspectable();

        _saveAllInspectables += SaveInspectable;
    }

    private void OnDestroy()
    {
        if (_inspectableDir == null) return;
        _saveAllInspectables -= SaveInspectable;
    }


    private void LoadInspectable()
    {
        // Already loaded
        if (_inspectableDir == null || _inspecData != null) return;

        try
        {
            // Read the JSON file and deserialize it + Set the data variable
            string json = File.ReadAllText(InspectableFilePath);
            _inspecData = JsonUtility.FromJson<InspecData>(json) ?? throw new Exception($"Failed to parse inspectable data from {InspectableFilePath}");
        }
        catch (Exception e)
        {
            Debug.Log($"Failed to load inspectable data for {name}: {e.Message}");

            // Initialize with default values if load fails
            _inspecData = new InspecData();
        }

        // Set the isFound flag based on the loaded data
        isFound = _inspecData.Found;
    }

    public void SaveInspectable()
    {
        // Only if has been found
        if (!isFound) return;

        // Save data variable
        _inspecData ??= new InspecData();
        _inspecData.Found = true;

        try
        {
            // Convert the data to JSON
            string json = JsonUtility.ToJson(_inspecData);

            // Create the directory if it doesn't exist
            if (!Directory.Exists(_inspectableDir)) Directory.CreateDirectory(_inspectableDir);

            // Save the JSON to a file
            File.WriteAllText(InspectableFilePath, json);
        }
        catch (Exception e)
        {
            Debug.Log($"Failed to save inspectable data for {name}: {e.Message}");
        }
    }


    [Serializable]
    private class InspecData
    {
        public bool Found = false;
    }
}