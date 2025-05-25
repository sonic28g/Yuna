using StarterAssets;
using UnityEngine;

public class Checkpoint : InteractableObject
{
    public bool isActiveCheckpoint = false;

    public override void Interact()
    {
        UIManager.instance.ShowInteractionText("Checkpoint saved...");
        CheckpointManager.Instance.SetCheckpoint(transform.position);
    }
}