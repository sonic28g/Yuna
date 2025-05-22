using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown cascadeDropdown;
    public TMP_Dropdown msaaDropdown;

    private bool isUpdatingUI = false;

    void Start()
    {
        UpdateUIFromCurrentSettings();

        qualityDropdown.onValueChanged.AddListener(OnQualityPresetChanged);
        cascadeDropdown.onValueChanged.AddListener(OnCascadeChanged);
        msaaDropdown.onValueChanged.AddListener(OnMSAAChanged);
    }

    void OnQualityPresetChanged(int index)
    {
        QualitySettings.SetQualityLevel(index, true);
        if (index != GetCustomIndex())
        {
            UpdateUIFromCurrentSettings();
        }
    }

    void OnCascadeChanged(int index)
    {
        if (isUpdatingUI) return;

        int value = index switch
        {
            0 => 1,
            1 => 2,
            2 => 4,
            _ => 1
        };

        ApplyOverrideToCustom(nameof(UniversalRenderPipelineAsset.shadowCascadeCount), value);
    }

    void OnMSAAChanged(int index)
    {
        if (isUpdatingUI) return;

        int value = index switch
        {
            0 => 1,
            1 => 2,
            2 => 4,
            3 => 8,
            _ => 1
        };

        ApplyOverrideToCustom(nameof(UniversalRenderPipelineAsset.msaaSampleCount), value);
    }

    void ApplyOverrideToCustom(string changedProperty, int newValue)
    {
        int currentIndex = QualitySettings.GetQualityLevel();
        int customIndex = GetCustomIndex();

        if (currentIndex != customIndex)
        {
            var from = QualitySettings.GetRenderPipelineAssetAt(currentIndex) as UniversalRenderPipelineAsset;
            var custom = QualitySettings.GetRenderPipelineAssetAt(customIndex) as UniversalRenderPipelineAsset;

            if (from == null || custom == null) return;

            // Copiar o estado atual
            custom.shadowCascadeCount = from.shadowCascadeCount;
            custom.msaaSampleCount = from.msaaSampleCount;

            // Aplicar override
            if (changedProperty == nameof(UniversalRenderPipelineAsset.shadowCascadeCount))
                custom.shadowCascadeCount = newValue;
            else if (changedProperty == nameof(UniversalRenderPipelineAsset.msaaSampleCount))
                custom.msaaSampleCount = newValue;

            QualitySettings.SetQualityLevel(customIndex, true);
            qualityDropdown.SetValueWithoutNotify(customIndex);
        }
        else
        {
            var custom = QualitySettings.GetRenderPipelineAssetAt(customIndex) as UniversalRenderPipelineAsset;
            if (changedProperty == nameof(UniversalRenderPipelineAsset.shadowCascadeCount))
                custom.shadowCascadeCount = newValue;
            else if (changedProperty == nameof(UniversalRenderPipelineAsset.msaaSampleCount))
                custom.msaaSampleCount = newValue;
        }

        UpdateUIFromCurrentSettings(); // para manter sincronizado
    }

    void UpdateUIFromCurrentSettings()
    {
        var urp = QualitySettings.GetRenderPipelineAssetAt(QualitySettings.GetQualityLevel()) as UniversalRenderPipelineAsset;        
        
        if (urp == null) return;

        isUpdatingUI = true;

        cascadeDropdown.SetValueWithoutNotify(urp.shadowCascadeCount switch
        {
            1 => 0,
            2 => 1,
            4 => 2,
            _ => 0
        });

        msaaDropdown.SetValueWithoutNotify(urp.msaaSampleCount switch
        {
            1 => 0,
            2 => 1,
            4 => 2,
            8 => 3,
            _ => 0
        });

        qualityDropdown.SetValueWithoutNotify(QualitySettings.GetQualityLevel());


        isUpdatingUI = false;
    }

    int GetCustomIndex() => 4;
}
