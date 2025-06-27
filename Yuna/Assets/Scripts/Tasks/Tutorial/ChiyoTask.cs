using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Chiyo")]
public class ChiyoTask : TaskData
{
    public override void StartTask()
    {
        completed = false;
    }

    public override bool CheckIfCompleted()
    {
        return completed;
    }

}
