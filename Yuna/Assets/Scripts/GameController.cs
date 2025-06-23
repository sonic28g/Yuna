using UnityEngine;
using UnityEngine.UI;
using StarterAssets;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject tutorial;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (DialogueManager.Instance.HasSeenDialogue("dialogue0"))
        {
            tutorial.SetActive(true);
        }
        else
        {
            tutorial.SetActive(false);
        }
    }

    public void StartDialogue(DialogueInteractable dialogueInteractable)
    {
        dialogueInteractable.Interact();
    }

}
