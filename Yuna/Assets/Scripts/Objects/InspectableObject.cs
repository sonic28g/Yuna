using UnityEngine;

public class InspectableObject : InteractableObject
{
    public override void Interact()
    {
        Debug.Log("Analisaste o objeto: " + objectName);
        UIManager.instance.ShowPickupText(objectName);
    }
}
