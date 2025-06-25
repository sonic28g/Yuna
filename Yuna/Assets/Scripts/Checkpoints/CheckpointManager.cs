using System.Collections;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    [SerializeField] private Vector3 lastCheckpointPos;
    [SerializeField] GameObject player;

    [SerializeField] GameObject foundPanel;

    [Header("Sound Settings")]
    [SerializeField] private AudioClip _checkpointSound;
    private AudioSource _audioSource;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _audioSource = GetComponent<AudioSource>();
        lastCheckpointPos = player.transform.position;
    }


    public void SetCheckpoint(bool playSound = true)
    {
        lastCheckpointPos = player.transform.position;

        EnemyController.SaveAllEnemies();
        NPCController.SaveAllNPCs();
        DialogueSet.SaveAllDialogueSets();
        if (GameController.Instance != null) GameController.Instance.SaveKanzashis();
        if (InventoryManager.instance != null) InventoryManager.instance.SaveInventory();

        if (playSound) PlayCheckpointSound();
    }

    private void PlayCheckpointSound()
    {
        if (_audioSource == null || _checkpointSound == null) return;
        _audioSource.PlayOneShot(_checkpointSound);
    }


    public void RespawnPlayer()
    {
        StartCoroutine(RespawnCoroutine());
    }

    public IEnumerator RespawnCoroutine()
    {
        foundPanel.SetActive(true);
        foundPanel.GetComponent<Animator>().SetTrigger("found");
        yield return new WaitForSeconds(2);

        player.transform.position = lastCheckpointPos;

        EnemyController.ResetAllEnemies();
        NPCController.ResetAllNPCs();
        if (GameController.Instance != null) GameController.Instance.ResetKanzashis();
        if (InventoryManager.instance != null) InventoryManager.instance.ResetInventory();

        yield return new WaitForSeconds(3);
        foundPanel.SetActive(false);
    }
}