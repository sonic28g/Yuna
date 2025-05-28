using System.Collections;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    [SerializeField] private Vector3 lastCheckpointPos;
    [SerializeField] GameObject player;

    [SerializeField] GameObject foundPanel;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);


        SetCheckpoint();
    }

    public void SetCheckpoint()
    {
        lastCheckpointPos = player.transform.position;
    }

    public void RespawnPlayer()
    {
        StartCoroutine(RespawnCoroutine());

        // Aqui podes adicionar: reset de estado, desativar alerta de inimigos, etc.
    }

    public IEnumerator RespawnCoroutine()
    {
        foundPanel.SetActive(true);
        foundPanel.GetComponent<Animator>().SetTrigger("found");
        yield return new WaitForSeconds(2);

        player.transform.position = lastCheckpointPos;
        
        yield return new WaitForSeconds(3);
        foundPanel.SetActive(false);
    }
}