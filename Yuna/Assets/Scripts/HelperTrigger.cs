using UnityEngine;

public class HelperTrigger : MonoBehaviour
{
    [SerializeField] string helperTitle;
    [SerializeField] string helperText;
    [SerializeField] Sprite helperImage;

    [SerializeField] private bool triggerBeforeTutorialStep = false;
    [SerializeField] private int tutorialStep = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (triggerBeforeTutorialStep && TutorialManager.Instance != null && TutorialManager.Instance.currentIndex >= tutorialStep) return;

        if (other.CompareTag("Player"))
        {
            UIManager.instance.ShowHelper(helperTitle, helperText, helperImage);
            Destroy(gameObject);
        }
    }
}
