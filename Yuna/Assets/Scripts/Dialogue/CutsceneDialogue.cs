using UnityEngine;
using UnityEngine.Playables;

public class CutsceneDialogue : MonoBehaviour
{
    public PlayableDirector director;
    public DialogueSet dialogue;

    private void Awake()
    {
        dialogue.InitDialogueSet();
    }

    private void Start()
    {
        if (DialogueManager.Instance.HasSeenDialogue(dialogue.DialogueId))
        {
            director.initialTime = director.duration;
            director.Play();
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (DialogueManager.Instance.HasSeenDialogue(dialogue.DialogueId))
        {
            director.Play();
            Destroy(gameObject);
        }

    }
}
