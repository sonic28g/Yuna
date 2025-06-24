using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    public PlayableDirector director;
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            director.Play();
            Destroy(gameObject);
        }
    }
}
