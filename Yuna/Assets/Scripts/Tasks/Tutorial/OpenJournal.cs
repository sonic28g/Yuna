using UnityEngine;
using StarterAssets;
using UnityEngine.InputSystem;


[CreateAssetMenu(menuName = "Tutorial/OpenJournal")]
public class OpenJournal : TaskData
{
    private StarterAssetsInputs _inputs;
    [SerializeField] string helperTitle;
    [SerializeField] string helperText;
    [SerializeField] Sprite helperImage;
    
    public override void StartTask()
    {
        completed = false;
        _inputs = GameObject.FindWithTag("Player").GetComponent<StarterAssetsInputs>();

        var playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        var scanBinding = playerInput.actions["TriggerJournal"].bindings[0]; // Pega a primeira tecla atribuída
        string openJournal = InputControlPath.ToHumanReadableString(scanBinding.effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);

        helperText = $"Yuna can check all the information on Yuna's Journal by pressing {openJournal}.";
        UIManager.instance.ShowHelper(helperTitle, helperText, helperImage);
    }

    public override bool CheckIfCompleted()
    {
        if (MenuController.Instance.showingJournal)
        {
            completed = true;
        }

        return completed;
    }

}