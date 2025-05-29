using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/MoveTask")]
public class MoveTask : TaskData
{
    private Vector3 lastPosition;

    public override void StartTask()
    {
        lastPosition = GameObject.FindWithTag("Player").transform.position;
        completed = false;
    }

    public override bool CheckIfCompleted()
    {
        var player = GameObject.FindWithTag("Player").transform;
        if (Vector3.Distance(player.position, lastPosition) > 1f)
        {
            completed = true;
        }
        return completed;
    }
}
