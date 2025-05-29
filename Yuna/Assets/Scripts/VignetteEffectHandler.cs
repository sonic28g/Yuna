using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignetteEffectHandler : MonoBehaviour
{
    [SerializeField] private Volume volume;
    [SerializeField] private float transitionSpeed = 3f;

    [Header("Default Vignette Values")]
    [SerializeField] private Color defaultColor = Color.black;
    [SerializeField] private float defaultIntensity = 0f;
    [SerializeField] private float defaultSmoothness = 0.5f;

    [Header("Aiming Vignette Values")]
    [SerializeField] private Color aimingColor = new Color(0.5f, 0f, 0.5f);
    [SerializeField] private float aimingIntensity = 0.5f;
    [SerializeField] private float aimingSmoothness = 0.9f;

    private Vignette vignette;

    private Color targetColor;
    private float targetIntensity;
    private float targetSmoothness;

    private void Awake()
    {
        if (volume.profile.TryGet(out vignette))
        {
            // Inicializa os valores default do Inspector
            defaultColor = vignette.color.value;
            defaultIntensity = vignette.intensity.value;
            defaultSmoothness = vignette.smoothness.value;

            ResetVignette();
        }
        else
        {
            Debug.LogWarning("Vignette não encontrada no volume!");
        }
    }

    void Update()
    {
        if (vignette == null) return;

        vignette.color.value = Color.Lerp(vignette.color.value, targetColor, Time.deltaTime * transitionSpeed);
        vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, targetIntensity, Time.deltaTime * transitionSpeed);
        vignette.smoothness.value = Mathf.Lerp(vignette.smoothness.value, targetSmoothness, Time.deltaTime * transitionSpeed);
    }

    public void SetAimingEffect()
    {
        targetColor = aimingColor;
        targetIntensity = aimingIntensity;
        targetSmoothness = aimingSmoothness;
    }

    public void ResetVignette()
    {
        targetColor = defaultColor;
        targetIntensity = defaultIntensity;
        targetSmoothness = defaultSmoothness;
    }
}
