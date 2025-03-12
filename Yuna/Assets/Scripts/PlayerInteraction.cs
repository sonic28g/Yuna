using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 2f;
    public Transform playerPosition;
    public LayerMask interactableLayer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            InteractWithObject();
        }
    }

    private void InteractWithObject()
    {
        Collider[] hitColliders = Physics.OverlapSphere(playerPosition.position, interactionRange, interactableLayer);
        
        foreach (Collider hitCollider in hitColliders)
        {
            InteractableObject interactable = hitCollider.GetComponent<InteractableObject>();
            if (interactable != null)
            {
                interactable.Interact();
                return;
            }
        }
    }
}