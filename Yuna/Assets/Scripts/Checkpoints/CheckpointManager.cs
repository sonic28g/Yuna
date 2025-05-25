using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    [SerializeField] private Vector2 lastCheckpointPos;
    private GameObject player;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void SetCheckpoint(Vector2 pos)
    {
        lastCheckpointPos = pos;
    }

    public void RespawnPlayer()
    {
        player.transform.position = lastCheckpointPos;

        // Aqui podes adicionar: reset de estado, desativar alerta de inimigos, etc.
    }
}