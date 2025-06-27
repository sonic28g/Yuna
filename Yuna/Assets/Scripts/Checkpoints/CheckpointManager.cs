using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    private string _playerDir;
    private string CheckpointFilePath => $"{_playerDir}/checkpoint.json";
    private CheckpointData _checkData;

    private static readonly string PLAYER_TAG = "Player";
    [SerializeField] GameObject player;
    [SerializeField] GameObject foundPanel;
    [SerializeField] GameObject checkpointPanel;


    [Header("Sound Settings")]
    [SerializeField] private AudioClip _checkpointSound;
    private AudioSource _audioSource;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (player == null) player = GameObject.FindGameObjectWithTag(PLAYER_TAG);
        _audioSource = GetComponent<AudioSource>();

        _playerDir = $"{Application.persistentDataPath}/Player";
        ResetCheckpoint();
    }


    public void SetCheckpoint(bool playSound = true)
    {
        SaveCheckpoint();

        EnemyController.SaveAllEnemies();
        NPCController.SaveAllNPCs();
        DialogueSet.SaveAllDialogueSets();
        InspectableData.SaveAllInspectables();
        if (GameController.Instance != null) GameController.Instance.SaveKanzashis();
        if (InventoryManager.instance != null) InventoryManager.instance.SaveInventory();
        if (TutorialManager.Instance != null) TutorialManager.Instance.SaveTutorial();

        if (playSound) PlayCheckpointSound();
    }

    private void PlayCheckpointSound()
    {
        if (_audioSource == null || _checkpointSound == null) return;
        _audioSource.PlayOneShot(_checkpointSound);
    }


    public void RespawnPlayer()
    {
        StartCoroutine(RespawnCoroutine());
    }

    public IEnumerator RespawnCoroutine()
    {
        foundPanel.SetActive(true);
        foundPanel.GetComponent<Animator>().SetTrigger("found");
        yield return new WaitForSeconds(1);

        ResetCheckpoint();

        EnemyController.ResetAllEnemies();
        NPCController.ResetAllNPCs();
        if (GameController.Instance != null) GameController.Instance.ResetKanzashis();
        if (InventoryManager.instance != null) InventoryManager.instance.ResetInventory();

        yield return new WaitForSeconds(2);
        foundPanel.SetActive(false);

    }


    public void ResetCheckpoint()
    {
        LoadFromFile();
        if (_checkData == null || player == null) return;

        // Set player position and rotation from checkpoint data
        player.transform.SetPositionAndRotation(_checkData.PlayerPosition, _checkData.PlayerRotation);
    }

    private void LoadFromFile()
    {
        // Already loaded
        if (_checkData != null) return;

        try
        {
            // Read the JSON file and deserialize it + Set the data variable
            string json = File.ReadAllText(CheckpointFilePath);
            _checkData = JsonUtility.FromJson<CheckpointData>(json) ?? throw new System.Exception($"Failed to parse checkpoint data from {CheckpointFilePath}");
        }
        catch (System.Exception e)
        {
            Debug.Log($"Failed to load checkpoint data for {name}: {e.Message}");

            // Initialize with default values if load fails
            _checkData = new CheckpointData
            {
                PlayerPosition = player != null ? player.transform.position : Vector3.zero,
                PlayerRotation = player != null ? player.transform.rotation : Quaternion.identity
            };
        }
    }


    public void SaveCheckpoint()
    {
        // Save data variable
        _checkData ??= new();
        _checkData.PlayerPosition = player != null ? player.transform.position : Vector3.zero;
        _checkData.PlayerRotation = player != null ? player.transform.rotation : Quaternion.identity;

        try
        {
            // Convert the data to JSON
            string json = JsonUtility.ToJson(_checkData);

            // Create the directory if it doesn't exist
            if (!Directory.Exists(_playerDir)) Directory.CreateDirectory(_playerDir);

            StartCoroutine(showAndHide(checkpointPanel, 2f));

            // Save the JSON to a file
            File.WriteAllText(CheckpointFilePath, json);
        }
        catch (System.Exception e)
        {
            Debug.Log($"Failed to save checkpoint data for {name}: {e.Message}");
        }
    }

    IEnumerator showAndHide(GameObject gameObject, float time)
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }


    [System.Serializable]
    private class CheckpointData
    {
        public Vector3 PlayerPosition;
        public Quaternion PlayerRotation;
    }
}