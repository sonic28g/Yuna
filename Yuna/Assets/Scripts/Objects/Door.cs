using UnityEngine;

public class Door : InteractableObject
{
    public Animator animator; // Referência ao Animator da porta
    private bool isOpen = false; // Estado da porta

    public override void Interact()
    {
        if (!isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    private void OpenDoor()
    {
        isOpen = true;
        //animator.SetTrigger("Open");
    }

    private void CloseDoor()
    {
        isOpen = false;
        //animator.SetTrigger("Close");
    }
}

