using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private BGMType _bgmType = BGMType.YunaHouse;
    public GameObject tutorial;


    private void OnEnable()
    {
        if (BGMPlayer.Instance != null) BGMPlayer.Instance.Play(_bgmType);
    }


    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    public void StartDialogue(DialogueInteractable dialogueInteractable)
    {
        dialogueInteractable.Interact();
    }

}
