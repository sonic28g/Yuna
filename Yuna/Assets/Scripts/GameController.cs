using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using TMPro;
using UnityEngine.Playables;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [SerializeField] private BGMType _defaultBGMType = BGMType.Outside;
    private readonly List<BGMTypeAreaTrigger> _currentBGMAreas = new();
    
    public GameObject tutorial;
    [SerializeField] private PlayableDirector _director;

    private string _playerDir;
    private string KanzashiFilePath => $"{_playerDir}/kanzashis.json";
    private KanzashiData _kanzashiData;
    [SerializeField] private GameObject _sceneKanzashi;
    private const string KANZASHI_TAG = "Kanzashi";

    [SerializeField] InspectableData letter;
    [SerializeField] InspectableData diary;
    [SerializeField] GameObject thoughtPanel;
    [SerializeField] RoomCheck room;
    [SerializeField] Diary yunasLetter;

    private void Awake()
    {
        Instance = this;
        BGMTypeAreaTrigger.OnBGMTypeAreaChanged += HandleBGMAreaChanged;

        _playerDir = $"{Application.persistentDataPath}/Player";
        ResetKanzashis();
    }

    private void OnDestroy() => BGMTypeAreaTrigger.OnBGMTypeAreaChanged -= HandleBGMAreaChanged;


    private void HandleBGMAreaChanged(BGMTypeAreaTrigger trigger, bool entered)
    {
        if (trigger == null) return;

        // Add or remove the trigger from the list
        if (entered && !_currentBGMAreas.Contains(trigger))
            _currentBGMAreas.Add(trigger);
        else if (!entered && _currentBGMAreas.Contains(trigger))
            _currentBGMAreas.Remove(trigger);

        UpdateCurrentArea();
    }

    private void UpdateCurrentArea()
    {
        if (BGMPlayer.Instance == null) return;

        // Order by BGMType priority and select the highest one
        // If no areas are found, use the default BGM type
        BGMType bgmType = _currentBGMAreas
            .Select(a => a.BGMType)
            .OrderByDescending(a => (int)a)
            .DefaultIfEmpty(_defaultBGMType)
            .FirstOrDefault();

        BGMPlayer.Instance.Play(bgmType);
    }


    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        yunasLetter.enabled = false;

        if (_director == null) return;
        if (TutorialManager.Instance == null || TutorialManager.Instance.currentIndex == 0) _director.Play();
        else
        {
            _director.initialTime = _director.duration;
            _director.Play();
        }
    }


    private void Update() {
        if (letter.isFound && diary.isFound)
        {
            thoughtPanel.SetActive(true);
            TextMeshProUGUI text = thoughtPanel.GetComponentInChildren<TextMeshProUGUI>();
            text.text = "I have all the evidences to save my father. I need to get back to my room.";

            if (room.isInRoom == true)
            {
                text.text = "I need to write to the shogun and send him all the evidences I found.";
                yunasLetter.enabled = true;
            }
        }
    }

    public void StartDialogue(DialogueInteractable dialogueInteractable)
    {
        dialogueInteractable.Interact();
    }


    private GameObject[] GetKanzashiObjects() => GameObject.FindGameObjectsWithTag(KANZASHI_TAG);
    private KanzashiData.KanzashiEntry[] GetKanzashiEntries() => GetKanzashiObjects()
        .Select(go => new KanzashiData.KanzashiEntry {
            Position = go.transform.position,
            Rotation = go.transform.rotation
        }).ToArray();


    public void ResetKanzashis()
    {
        if (_sceneKanzashi == null) return;
        LoadFromFile();

        // Clear existing kanzashi objects in the scene
        GameObject[] existingKanzashis = GetKanzashiObjects();
        foreach (GameObject kanzashi in existingKanzashis) Destroy(kanzashi);

        // Instantiate kanzashi objects based on collected entries
        if (_kanzashiData == null || _kanzashiData.KanzashiEntries == null) return;
        foreach (var entry in _kanzashiData.KanzashiEntries) Instantiate(_sceneKanzashi, entry.Position, entry.Rotation);
    }

    private void LoadFromFile()
    {
        // Already loaded
        if (_kanzashiData != null) return;

        try
        {
            // Read the JSON file and deserialize it + Set the data variable
            string json = File.ReadAllText(KanzashiFilePath);
            _kanzashiData = JsonUtility.FromJson<KanzashiData>(json) ?? throw new System.Exception($"Failed to parse kanzashi data from {KanzashiFilePath}");
        }
        catch (System.Exception e)
        {
            Debug.Log($"Failed to load kanzashi data for {name}: {e.Message}");

            // Initialize with default values if load fails
            _kanzashiData = new KanzashiData
            {
                KanzashiEntries = GetKanzashiEntries()
            };
        }
    }


    public void SaveKanzashis()
    {
        // Save data variable
        _kanzashiData ??= new();
        _kanzashiData.KanzashiEntries = GetKanzashiEntries();

        try
        {
            // Convert the data to JSON
            string json = JsonUtility.ToJson(_kanzashiData);

            // Create the directory if it doesn't exist
            if (!Directory.Exists(_playerDir)) Directory.CreateDirectory(_playerDir);

            // Save the JSON to a file
            File.WriteAllText(KanzashiFilePath, json);
        }
        catch (System.Exception e)
        {
            Debug.Log($"Failed to save kanzashi data for {name}: {e.Message}");
        }
    }


    [System.Serializable]
    private class KanzashiData
    {
        public KanzashiEntry[] KanzashiEntries;

        [System.Serializable]
        public class KanzashiEntry
        {
            public Vector3 Position;
            public Quaternion Rotation;
        }
    }
}
