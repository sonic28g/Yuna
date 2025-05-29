using UnityEngine;

public enum TaskType
{
    Tutorial,
    Story
}

[CreateAssetMenu(fileName = "New Task", menuName = "Tasks/Task")]
public abstract class TaskData : ScriptableObject
{
    public string taskID;
    public TaskType type;
    public string title;
    [TextArea] public string description;

    public bool completed = true;

    public abstract void StartTask();
    public abstract bool CheckIfCompleted();
    
}
