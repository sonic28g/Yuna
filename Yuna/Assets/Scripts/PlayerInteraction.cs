using StarterAssets;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private StarterAssetsInputs starterAssetsInputs;
    public float interactionRange = 2f;
    public Transform playerPosition;
    public LayerMask interactableLayer;

    private InteractableObject nearbyObject = null;

    private void Awake() 
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        DetectNearbyObject();
        
        if (starterAssetsInputs.interact && nearbyObject != null)
        {
            nearbyObject.Interact();
            starterAssetsInputs.interact = false;
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
