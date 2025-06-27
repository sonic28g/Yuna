using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Tutorial/ScanRoom")]
public class ScanRoom : TaskData
{
    private StarterAssetsInputs _inputs;

    [SerializeField] string helperTitle;
    [SerializeField] string helperText;
    [SerializeField] Sprite helperImage;


    public override void StartTask()
    {
        var playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        var scanBinding = playerInput.actions["Scan"].bindings[0]; // Pega a primeira tecla atribuída
        string scanKey = InputControlPath.ToHumanReadableString(scanBinding.effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);

        helperText = $"Yuna can enter focus mode. Press {scanKey} to scan the room. Yuna will highlight objects and enemies.";

        _inputs = GameObject.FindWithTag("Player").GetComponent<StarterAssetsInputs>();
        UIManager.instance.ShowHelper(helperTitle, helperText, helperImage);

        completed = false;
    }

    public override bool CheckIfCompleted()
    {
        if (_inputs.scan)
            completed = true;

        return completed;
    }
}