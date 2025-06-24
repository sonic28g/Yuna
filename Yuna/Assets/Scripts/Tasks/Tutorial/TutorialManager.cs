using System.Collections;
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
        currentTask.completed = true;
    }
}
