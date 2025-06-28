using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class FadeInText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float fadeDuration = 2f;

    void Start()
    {
        text.alpha = 0;
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            text.alpha = timer / fadeDuration;
            yield return null;
        }
        text.alpha = 1f;
    }
}
