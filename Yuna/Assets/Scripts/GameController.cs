using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private BGMPlayer.BGMType _bgmType = BGMPlayer.BGMType.YunaHouse;
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


    private void Update()
    {

    }

    public void StartDialogue(DialogueInteractable dialogueInteractable)
    {
        dialogueInteractable.Interact();
    }

}
