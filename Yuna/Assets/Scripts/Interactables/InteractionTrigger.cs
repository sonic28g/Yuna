using UnityEngine;

public class InteractionTrigger : MonoBehaviour
{
    private PlayerInteraction playerInteraction;

    private void Awake()
    {
        playerInteraction = GetComponentInParent<PlayerInteraction>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & LayerMask.GetMask("Interactable")) != 0)
        {
            InteractableObject interactable = other.GetComponent<InteractableObject>();
            if (interactable != null)
            {
                playerInteraction.SetNearbyObject(interactable);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<InteractableObject>() != null)
        {
            playerInteraction.ClearNearbyObject();
        }
    }
}