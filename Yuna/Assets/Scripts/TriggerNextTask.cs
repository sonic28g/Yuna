using UnityEngine;

public class TriggerNextTask : MonoBehaviour
{

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player"))
            TutorialManager.Instance.MarkCompleted();
    }
}
