using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/LeaveRoomTask")]
public class LeaveRoom : TaskData
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
