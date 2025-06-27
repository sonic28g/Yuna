using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class ButtonZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float zoomScale = 1.2f; // Quanto vai aumentar
    [SerializeField] private float zoomDuration = 0.1f; // Velocidade da transição

    private Vector3 originalScale;
    private Coroutine zoomCoroutine;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartZoom(originalScale * zoomScale);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartZoom(originalScale);
    }

    private void StartZoom(Vector3 targetScale)
    {
        if (zoomCoroutine != null)
            StopCoroutine(zoomCoroutine);

        zoomCoroutine = StartCoroutine(ZoomTo(targetScale));
    }

    private IEnumerator ZoomTo(Vector3 targetScale)
    {
        Vector3 startScale = transform.localScale;
        float time = 0f;

        while (time < zoomDuration)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, time / zoomDuration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }
}
