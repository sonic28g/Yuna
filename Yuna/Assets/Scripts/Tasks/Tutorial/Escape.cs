using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Escape")]
public class Escape : TaskData
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
