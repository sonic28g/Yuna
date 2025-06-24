using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/HearGuards")]
public class HearGuards : TaskData
{
    [SerializeField] string dialogueID;

    public override void StartTask()
    {
        completed = false;
    }

    public override bool CheckIfCompleted()
    {
        if (DialogueManager.Instance.HasSeenDialogue(dialogueID))
        {
            completed = true;
        }

        return completed;
    }

}