using System;
using UnityEngine;

public class InspectableObject : InteractableObject
{
    public InspectableData inspectableData;

    private void Start() {
        inspectableData.isFound = false;
    }

    public override void Interact()
    {
        UIManager.instance.ShowDiaryEntry();
        inspectableData.isFound = true;

        if (inspectableData.type.ToString().ToLower().Equals("clue"))
        {
            if (DiaryManager.Instance != null)
            {
                DiaryManager.Instance.UpdateDiary(inspectableData);
            }
        }

        if (inspectableData.type.ToString().ToLower() == "evidence")
        {
            if (EvidenceManager.Instance != null)
            {
                EvidenceManager.Instance.UpdateEvidence(inspectableData);
            }
        }

        inspectableData.isActive = false;
        gameObject.SetActive(false);
    }
}
