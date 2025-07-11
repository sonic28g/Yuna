using StarterAssets;
using UnityEngine;

public class Checkpoint : InteractableObject
{
    public bool isActiveCheckpoint = false;

    public override void Interact()
    {
        CheckpointManager.Instance.SetCheckpoint();
    }
}