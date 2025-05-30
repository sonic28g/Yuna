using UnityEngine;

public class WaterRespawn : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player")
        {
            // animação afogar
            // depois de uns segundos:
            CheckpointManager.Instance.RespawnPlayer();
        }
    }
}
