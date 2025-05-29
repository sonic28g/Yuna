using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/LeaveRoomTask")]
public class LeaveRoom : TaskData
{
    public override void StartTask()
    {
        completed = false;
        Door.OnDoorOpened += OnDoorOpened;
    }

    public override bool CheckIfCompleted()
    {
        return completed;
    }

    private void OnDoorOpened(Door door)
    {
        completed = true;
        Door.OnDoorOpened -= OnDoorOpened; // remover depois de completado
    }
}
