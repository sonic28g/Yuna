using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameController : MonoBehaviour
{
    [SerializeField] private BGMType _defaultBGMType = BGMType.Outside;
    private readonly List<BGMTypeAreaTrigger> _currentBGMAreas = new();
    
    public GameObject tutorial;


    private void Awake() => BGMTypeAreaTrigger.OnBGMTypeAreaChanged += HandleBGMAreaChanged;
    private void OnDestroy() => BGMTypeAreaTrigger.OnBGMTypeAreaChanged -= HandleBGMAreaChanged;


    private void HandleBGMAreaChanged(BGMTypeAreaTrigger trigger, bool entered)
    {
        if (trigger == null) return;

        // Add or remove the trigger from the list
        if (entered && !_currentBGMAreas.Contains(trigger))
            _currentBGMAreas.Add(trigger);
        else if (!entered && _currentBGMAreas.Contains(trigger))
            _currentBGMAreas.Remove(trigger);

        UpdateCurrentArea();
    }

    private void UpdateCurrentArea()
    {
        if (BGMPlayer.Instance == null) return;

        // Order by BGMType priority and select the highest one
        // If no areas are found, use the default BGM type
        BGMType bgmType = _currentBGMAreas
            .Select(a => a.BGMType)
            .OrderByDescending(a => (int)a)
            .DefaultIfEmpty(_defaultBGMType)
            .FirstOrDefault();

        BGMPlayer.Instance.Play(bgmType);
    }


    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    public void StartDialogue(DialogueInteractable dialogueInteractable)
    {
        dialogueInteractable.Interact();
    }

}
