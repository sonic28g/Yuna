using TMPro;
using UnityEngine;
using UnityEngine.UI; // necessário para Toggle

public class SettingsUI : MonoBehaviour
{
    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown cascadeDropdown;
    public TMP_Dropdown msaaDropdown;

    bool isUpdatingUI;

    void Start()
    {
        UpdateUI();

        qualityDropdown.onValueChanged.AddListener(index =>
        {
            SettingsManager.SetQualityLevel(index);
            UpdateUI();
        });

        cascadeDropdown.onValueChanged.AddListener(index =>
        {
            if (isUpdatingUI) return;
            SettingsManager.ApplyCascadeCount(index switch { 0 => 1, 1 => 2, 2 => 3, 3 => 4, _ => 1 });
            UpdateUI();
        });

        msaaDropdown.onValueChanged.AddListener(index =>
        {
            if (isUpdatingUI) return;
            SettingsManager.ApplyMSAA(index switch { 0 => 1, 1 => 2, 2 => 4, 3 => 8, _ => 1 });
            UpdateUI();
        });

    }

    void UpdateUI()
    {
        var urp = SettingsManager.GetCurrentURPAsset();
        if (urp == null) return;

        isUpdatingUI = true;

        cascadeDropdown.SetValueWithoutNotify(urp.shadowCascadeCount switch { 1 => 0, 2 => 1, 3 => 2, 4 => 3, _ => 0 });
        msaaDropdown.SetValueWithoutNotify(urp.msaaSampleCount switch { 1 => 0, 2 => 1, 4 => 2, 8 => 3, _ => 0 });
        qualityDropdown.SetValueWithoutNotify(QualitySettings.GetQualityLevel());

        isUpdatingUI = false;
    }
}