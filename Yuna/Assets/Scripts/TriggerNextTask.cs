using UnityEngine;

public class TriggerNextTask : MonoBehaviour
{
    [SerializeField] private bool triggerBeforeTutorialStep = false;
    [SerializeField] private int tutorialStep = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (triggerBeforeTutorialStep && TutorialManager.Instance != null && TutorialManager.Instance.currentIndex >= tutorialStep) return;

        if (other.CompareTag("Player"))
            TutorialManager.Instance.MarkCompleted();
    }
}
