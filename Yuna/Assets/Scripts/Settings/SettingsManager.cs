using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public static class SettingsManager
{
    // Índice do preset "Custom" nas definições de qualidade
    public static int CustomIndex => 4;

    private static bool highContrastEnabled = false;

    // Define o nível de qualidade atual usando o índice (aplica as alterações imediatamente)
    public static void SetQualityLevel(int index)
    {
        QualitySettings.SetQualityLevel(index, true);
        GameSettings settings = new GameSettings { qualityLevel = index };
        settings.SaveToFile();
    }

    // Obtém o índice do nível de qualidade atualmente ativo
    public static int GetCurrentQualityIndex()
    {
        return QualitySettings.GetQualityLevel();
    }

    // Obtém o URP Asset associado ao nível de qualidade atual
    public static UniversalRenderPipelineAsset GetCurrentURPAsset()
    {
        return QualitySettings.GetRenderPipelineAssetAt(GetCurrentQualityIndex()) as UniversalRenderPipelineAsset;
    }

    // Aplica o número de cascatas de sombras ao preset "Custom"
    public static void ApplyCascadeCount(int cascadeCount)
    {
        ApplyOverride(nameof(UniversalRenderPipelineAsset.shadowCascadeCount), cascadeCount);
    }

    // Aplica a quantidade de amostragem MSAA (Anti-Aliasing) ao preset "Custom"
    public static void ApplyMSAA(int msaaSamples)
    {
        ApplyOverride(nameof(UniversalRenderPipelineAsset.msaaSampleCount), msaaSamples);
    }

    // Ativa ou desativa o modo de alto contraste
    public static void ApplyHighContrast(bool enable)
    {
        highContrastEnabled = enable;

        // Tenta encontrar o GameObject com o Volume
        var volumeObj = GameObject.Find("Global Volume");
        if (volumeObj == null)
        {
            Debug.LogWarning("HighContrastVolume não encontrado na cena.");
            return;
        }

        // Ativa/desativa o GameObject
        volumeObj.SetActive(enable);

        // Ajusta o contraste diretamente se o Volume e o efeito existirem
        if (volumeObj.TryGetComponent<Volume>(out var volume) && volume.profile != null)
        {
            if (volume.profile.TryGet<ColorAdjustments>(out var colorAdjustments))
            {
                colorAdjustments.active = enable;
                colorAdjustments.contrast.value = enable ? 50f : 0f; // Pode ir de -100 a 100
            }
            else
            {
                Debug.LogWarning("ColorAdjustments não está presente no perfil do Volume.");
            }
        }

        // (Opcional) Guardar nos settings
        // new GameSettings { highContrast = enable }.SaveToFile();
    }

    public static bool IsHighContrastEnabled()
    {
        return highContrastEnabled;
    }

    // Método genérico para aplicar alterações ao URP Asset "Custom"
    private static void ApplyOverride(string propertyName, int newValue)
    {
        int currentIndex = GetCurrentQualityIndex();  // Índice atual do preset ativo
        int customIndex = CustomIndex;                // Índice do preset customizado

        var currentAsset = QualitySettings.GetRenderPipelineAssetAt(currentIndex) as UniversalRenderPipelineAsset;
        var customAsset = QualitySettings.GetRenderPipelineAssetAt(customIndex) as UniversalRenderPipelineAsset;

        if (currentAsset == null || customAsset == null)
        {
            Debug.LogWarning("URP Asset(s) não encontrados.");
            return;
        }

        // Copia os valores atuais para o Custom se ainda não estivermos nele
        if (currentIndex != customIndex)
        {
            customAsset.shadowCascadeCount = currentAsset.shadowCascadeCount;
            customAsset.msaaSampleCount = currentAsset.msaaSampleCount;
        }

        // Aplica o novo valor à propriedade especificada
        if (propertyName == nameof(UniversalRenderPipelineAsset.shadowCascadeCount))
            customAsset.shadowCascadeCount = newValue;
        else if (propertyName == nameof(UniversalRenderPipelineAsset.msaaSampleCount))
            customAsset.msaaSampleCount = newValue;

        // Ativa o preset Custom
        QualitySettings.SetQualityLevel(customIndex, true);
    }
}