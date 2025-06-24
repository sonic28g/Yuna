using StarterAssets;
using UnityEngine;

public class Checkpoint : InteractableObject
{
    public bool isActiveCheckpoint = false;

    public override void Interact()
    {
        print("checkpoint saved");
        CheckpointManager.Instance.SetCheckpoint();
    }
}