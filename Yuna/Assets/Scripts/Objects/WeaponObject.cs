using UnityEngine;

public class WeaponObject : InteractableObject
{
    public override void Interact()
    {
        Debug.Log("Pegaste a arma: " + objectName);
        Destroy(gameObject);
    }
}
