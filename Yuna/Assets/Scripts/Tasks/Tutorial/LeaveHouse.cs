using StarterAssets;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/LeaveHouse")]
public class LeaveHouse : TaskData
{
    public override void StartTask()
    {
        
        completed = false;
    }

    public override bool CheckIfCompleted()
    {
        bool leftHouse = GameObject.FindGameObjectWithTag("NoSprintZone").GetComponent<NoSprintZone>().isOnSprintZone;

        if (leftHouse)
            completed = true;

        return completed;
    }

}