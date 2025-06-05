using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    public string objectName;

    public abstract void Interact();
}
