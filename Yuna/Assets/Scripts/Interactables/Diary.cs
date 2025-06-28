using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Diary : InteractableObject
{
    [SerializeField] PlayableDirector finalScene;
    public override void Interact()
    {
        
        finalScene.Play();
    }
}
