using UnityEngine;
using UnityEngine.UI;
using StarterAssets;
using TMPro;

public class GameController : MonoBehaviour
{
    public DialogueInteractable dialogueInteractable;
    public GameObject tutorial;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (DialogueManager.Instance.IsDialogueActive)
        {
            tutorial.SetActive(false);
        }
        else
        {
            tutorial.SetActive(true);
        }
    }

    public void StartDialogue()
    {
        dialogueInteractable.Interact();
    }

}
