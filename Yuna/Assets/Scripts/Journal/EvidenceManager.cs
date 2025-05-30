using TMPro;
using UnityEngine;

public class EvidenceManager : MonoBehaviour
{
    public static EvidenceManager Instance { get; private set; }

    public TextMeshProUGUI evidence1Title;
    public TextMeshProUGUI evidence1Description;

    public TextMeshProUGUI evidence2Title;
    public TextMeshProUGUI evidence2Description;

    public TextMeshProUGUI evidence3Title;
    public TextMeshProUGUI evidence3Description;

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

    public void UpdateEvidence(InspectableData evidenceData)
    {
        if (evidenceData.isFound)
        {
            if (evidenceData.inspectableID.Equals("Evidence1"))
            {
                evidence1Title.text = evidenceData.inspectableTitle;
                evidence1Description.text = evidenceData.inspectableDescription;
            }
            else if (evidenceData.inspectableID.Equals("Evidence2"))
            {
                evidence2Title.text = evidenceData.inspectableTitle;
                evidence2Description.text = evidenceData.inspectableDescription;
            }
            else if (evidenceData.inspectableID.Equals("Evidence3"))
            {
                evidence3Title.text = evidenceData.inspectableTitle;
                evidence3Description.text = evidenceData.inspectableDescription;
            }
        }
    }


}
