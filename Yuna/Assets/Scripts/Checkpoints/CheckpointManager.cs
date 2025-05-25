using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    [SerializeField] private Vector3 lastCheckpointPos;
    private GameObject player;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void SetCheckpoint()
    {
        lastCheckpointPos = player.transform.position;
    }

    public void RespawnPlayer()
    {
        player.transform.position = lastCheckpointPos;

        // Aqui podes adicionar: reset de estado, desativar alerta de inimigos, etc.
    }
}