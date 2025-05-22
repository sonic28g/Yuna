using UnityEngine;
using UnityEngine.Rendering.Universal;

public static class SettingsManager
{
    // Índice do preset "Custom" nas definições de qualidade
    public static int CustomIndex => 4;

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

    // Método genérico para aplicar alterações ao URP Asset "Custom"
    private static void ApplyOverride(string propertyName, int newValue)
    {
        int currentIndex = GetCurrentQualityIndex();  // Índice atual do preset ativo
        int customIndex = CustomIndex;                // Índice do preset customizado

        // Obtemos os URP Assets do preset atual e do Custom
        var currentAsset = QualitySettings.GetRenderPipelineAssetAt(currentIndex) as UniversalRenderPipelineAsset;
        var customAsset = QualitySettings.GetRenderPipelineAssetAt(customIndex) as UniversalRenderPipelineAsset;

        // Se algum dos assets não for válido, mostramos aviso e saímos
        if (currentAsset == null || customAsset == null)
        {
            Debug.LogWarning("URP Asset(s) not found.");
            return;
        }

        // Se ainda não estivermos no preset Custom, copiamos os valores atuais para o Custom
        if (currentIndex != customIndex)
        {
            customAsset.shadowCascadeCount = currentAsset.shadowCascadeCount;
            customAsset.msaaSampleCount = currentAsset.msaaSampleCount;
        }

        // Aplicamos o novo valor à propriedade especificada
        if (propertyName == nameof(UniversalRenderPipelineAsset.shadowCascadeCount))
            customAsset.shadowCascadeCount = newValue;
        else if (propertyName == nameof(UniversalRenderPipelineAsset.msaaSampleCount))
            customAsset.msaaSampleCount = newValue;

        // Mudamos para o preset Custom, com as alterações aplicadas
        QualitySettings.SetQualityLevel(customIndex, true);
    }
}
