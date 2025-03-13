using UnityEngine;

public class WeaponObject : InteractableObject
{
    public override void Interact()
    {
        Debug.Log("Pegaste a arma: " + objectName);
        UIManager.instance.ShowPickupText(objectName);

        Destroy(gameObject);
    }
}
