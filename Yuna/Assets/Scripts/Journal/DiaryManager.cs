using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiaryManager : MonoBehaviour
{
    public static DiaryManager Instance { get; private set; }

    public TextMeshProUGUI diaryText;

    private bool noClues = true;

    private void Start() {
        diaryText.text = "No clues yet...";
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateDiary(InspectableData inspectableData)
    {
        if (noClues)
        {
            diaryText.text = "";
            noClues = false;
        }

        diaryText.text += inspectableData.inspectableTitle + ": " + inspectableData.inspectableDescription + "\n";
    
    }
}

