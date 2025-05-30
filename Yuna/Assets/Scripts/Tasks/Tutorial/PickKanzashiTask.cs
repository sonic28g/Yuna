using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/PickKanzashi")]
public class PickKanzashiTask : TaskData
{
    private Vector3 lastPosition;

    public override void StartTask()
    {
        completed = false;
    }

    public override bool CheckIfCompleted()
    {
        if (InventoryManager.instance.GetAmmo("Kanzashi") >= 1)
        {
            completed = true;
        }
        return completed;
    }
}
