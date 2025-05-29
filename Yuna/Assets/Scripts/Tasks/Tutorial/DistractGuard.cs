using StarterAssets;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/DistractGuard")]
public class DistractGuard : TaskData
{

    private StarterAssetsInputs _inputs;

    public override void StartTask()
    {
        _inputs = GameObject.FindWithTag("Player").GetComponent<StarterAssetsInputs>();
        completed = false;
    }

    public override bool CheckIfCompleted()
    {
        if (_inputs.aim && _inputs.shoot)
            completed = true;

        return completed;
    }

    private void OnDoorOpened(Door door)
    {
        completed = true;
        Door.OnDoorOpened -= OnDoorOpened; // remover depois de completado
    }
}
