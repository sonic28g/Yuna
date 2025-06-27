using UnityEngine;

public class LocationTrigger : MonoBehaviour
{
    [SerializeField] string zoneName;
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
            PlayerLocationManager.instance.UpdatePlayerZone(zoneName);
    }
}
