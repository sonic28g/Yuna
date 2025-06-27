using StarterAssets;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private StarterAssetsInputs starterAssetsInputs;
    private InteractableObject nearbyObject = null;

    private void Awake() 
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        if (starterAssetsInputs.interact && nearbyObject != null)
        {
            nearbyObject.Interact();
            starterAssetsInputs.interact = false;
        }
        else
        {
            starterAssetsInputs.interact = false;
        }
    }

    public void SetNearbyObject(InteractableObject obj)
    {
        nearbyObject = obj;
        UIManager.instance.ShowNearbyText(true, obj);
    }

    public void ClearNearbyObject()
    {
        UIManager.instance.ShowNearbyText(false, nearbyObject);
        nearbyObject = null;
    }
}