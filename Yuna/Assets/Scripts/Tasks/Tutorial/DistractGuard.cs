using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Tutorial/DistractGuard")]
public class DistractGuard : TaskData
{

    private StarterAssetsInputs _inputs;

    [SerializeField] string helperTitle;
    [SerializeField] string helperText;
    [SerializeField] Sprite helperImage;
    public override void StartTask()
    {
        var playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        var aimBinding = playerInput.actions["Aim"].bindings[0];
        var shootBinding = playerInput.actions["Shoot"].bindings[0];

        string aimKey = InputControlPath.ToHumanReadableString(aimBinding.effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        string shootKey = InputControlPath.ToHumanReadableString(shootBinding.effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);

        helperText = $"Yuna can aim to throw kanzashis. Press {aimKey} to aim and {shootKey} to throw. If a kanzashi lands close to a guard it will alert them.";
        UIManager.instance.ShowHelper(helperTitle, helperText, helperImage);

        _inputs = GameObject.FindWithTag("Player").GetComponent<StarterAssetsInputs>();

        completed = false;

    }

    public override bool CheckIfCompleted()
    {       
        return completed;
    }
}
