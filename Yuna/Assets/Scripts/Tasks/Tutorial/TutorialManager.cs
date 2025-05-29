using System.Collections;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance; // Singleton para acesso fácil
    public int currentIndex { get; private set; } = 0;

    private TaskData currentTask;
    public TutorialSequence tutorialSequence;
    public TextMeshProUGUI tutorialText;
    public GameObject tutorialPanel;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartNextTask();
    }

    void Update()
    {
        if (currentTask != null && currentTask.CheckIfCompleted())
        {
            currentIndex++;
            StartNextTask();
        }
    }

    void StartNextTask()
    {
        if (currentIndex >= tutorialSequence.tutorialTasks.Length)
        {

            currentTask = null;

            StartCoroutine(ShowTutorialCompletion());

            return;
        }

        currentTask = tutorialSequence.tutorialTasks[currentIndex];
        currentTask.StartTask();
        tutorialText.text = currentTask.description;
    }

    private IEnumerator ShowTutorialCompletion()
    {
        tutorialText.text = "Tutorial completed!";
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
        tutorialPanel.SetActive(false);
    }
}
