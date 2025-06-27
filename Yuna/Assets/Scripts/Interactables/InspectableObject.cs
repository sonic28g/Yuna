public class InspectableObject : InteractableObject
{
    public InspectableData inspectableData;

    private void Awake()
    {
        if (inspectableData == null) throw new System.Exception($"InspectableData is not assigned in {name}");
        // inspectableData.InitInspectable();
    }

    private void Start()
    {
        if (inspectableData.isFound) Interact();
    }

    public override void Interact()
    {
        UIManager.instance.ShowDiaryEntry();
        inspectableData.isFound = true;

        string type = inspectableData.type.ToString().ToLower();
        if (type.Equals("clue") && DiaryManager.Instance != null)
        {
            DiaryManager.Instance.UpdateDiary(inspectableData);
        }
        else if (type.Equals("evidence") && EvidenceManager.Instance != null)
        {
            EvidenceManager.Instance.UpdateEvidence(inspectableData);
        }

        inspectableData.isActive = false;
        gameObject.SetActive(false);
    }
}
