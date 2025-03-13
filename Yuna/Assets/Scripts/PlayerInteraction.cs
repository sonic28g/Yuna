using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 2f;
    public Transform playerPosition;
    public LayerMask interactableLayer;

    private InteractableObject nearbyObject = null;

    private void Update()
    {
        DetectNearbyObject();
        
        if (Input.GetKeyDown(KeyCode.E) && nearbyObject != null)
        {
            nearbyObject.Interact();
        }
    }

    private void DetectNearbyObject()
    {
        Collider[] hitColliders = Physics.OverlapSphere(playerPosition.position, interactionRange, interactableLayer);

        if (hitColliders.Length > 0)
        {
            nearbyObject = hitColliders[0].GetComponent<InteractableObject>();
            if (nearbyObject != null)
            {
                UIManager.instance.ShowNearbyText(true);
                return;
            }
        }

        nearbyObject = null;
        UIManager.instance.ShowNearbyText(false);
    }
}
