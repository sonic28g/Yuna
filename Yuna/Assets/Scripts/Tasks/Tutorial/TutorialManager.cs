using System.IO;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
    public int currentIndex { get; private set; } = 0;

    private TaskData currentTask;
    public TutorialSequence tutorialSequence;
    public TextMeshProUGUI tutorialText;
    public GameObject tutorialPanel;

    [Header("Sound Settings")]
    [SerializeField] private AudioClip _completionSound;
    private AudioSource _audioSource;


    void Awake()
    {
        Instance = this;
        _audioSource = GetComponent<AudioSource>();

        LoadTutorial();
    }

    void Start()
    {
        tutorialPanel.SetActive(true);
        StartNextTask();
    }

    void Update()
    {
        if (currentTask != null && currentTask.CheckIfCompleted())
        {
            currentIndex++;
            StartNextTask();

            PlayCompletionSound();
        }
    }

    public void StartNextTask()
    {
        if (currentIndex >= tutorialSequence.tutorialTasks.Length)
        {
            currentTask = null;
            tutorialPanel.SetActive(false);
            return;
        }

        currentTask = tutorialSequence.tutorialTasks[currentIndex];
        currentTask.StartTask();
        tutorialText.text = currentTask.description;
    }

    private void PlayCompletionSound()
    {
        if (_audioSource == null || _completionSound == null) return;
        _audioSource.PlayOneShot(_completionSound);
    }

    public void MarkCompleted()
    {
        if (currentTask == null) return;
        currentTask.completed = true;
        print("completed");
    }


    private void LoadTutorial()
    {
        string path = Application.persistentDataPath + "/Player/tutorial.json";
        TutorialData tutorialData = new();

        try
        {
            string json = File.ReadAllText(path);
            tutorialData = JsonUtility.FromJson<TutorialData>(json);
        }
        catch (System.Exception e)
        {
            Debug.Log("Failed to load tutorial data: " + e.Message);
        }

        currentIndex = tutorialData.CurrentIndex;
    }

    public void SaveTutorial()
    {
        string path = Application.persistentDataPath + "/Player/tutorial.json";
        string directory = Path.GetDirectoryName(path);

        TutorialData tutorialData = new() {
            CurrentIndex = currentIndex
        };

        try
        {
            string json = JsonUtility.ToJson(tutorialData);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            File.WriteAllText(path, json);
        }
        catch (System.Exception e)
        {
            Debug.Log("Failed to save tutorial data: " + e.Message);
        }
    }


    [System.Serializable]
    public class TutorialData
    {
        public int CurrentIndex;
    }
}
