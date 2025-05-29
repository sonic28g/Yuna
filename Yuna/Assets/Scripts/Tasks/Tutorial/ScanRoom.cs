using StarterAssets;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/ScanRoom")]
public class ScanRoom : TaskData
{
    private StarterAssetsInputs _inputs;

    public override void StartTask()
    {
        _inputs = GameObject.FindWithTag("Player").GetComponent<StarterAssetsInputs>();
        completed = false;
    }

    public override bool CheckIfCompleted()
    {
        if (_inputs.scan)
            completed = true;

        return completed;
    }
}