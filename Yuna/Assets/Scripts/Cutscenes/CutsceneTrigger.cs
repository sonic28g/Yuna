using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    public PlayableDirector director;

    [SerializeField] private bool triggerBeforeTutorialStep = false;
    [SerializeField] private int tutorialStep = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (director == null) return;
        if (triggerBeforeTutorialStep && TutorialManager.Instance != null && TutorialManager.Instance.currentIndex >= tutorialStep) return;

        if (other.CompareTag("Player"))
        {
            director.Play();
            Destroy(gameObject);
        }
    }
}
