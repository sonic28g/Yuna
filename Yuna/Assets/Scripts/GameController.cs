using UnityEngine;
using UnityEngine.UI;
using StarterAssets;
using TMPro;

public class GameController : MonoBehaviour
{
    public DialogueInteractable dialogueInteractable;

    void Start()
    {
        Cursor.visible = false; // Esconde o cursor
        Cursor.lockState = CursorLockMode.Locked; // Tranca o cursor ao centro do ecrã
        dialogueInteractable.Interact();

    }

    private void Update() 
    {

    }



}
