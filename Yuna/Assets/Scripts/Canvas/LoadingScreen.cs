using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public Slider loadingBar;
    public Text loadingText; // Opcional

    void Start()
    {
        StartCoroutine(LoadSceneAsync("Game"));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // Evita ativar logo a cena (se quiseres fazer fade depois)
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBar.value = progress;

            if (loadingText != null)
                loadingText.text = Mathf.RoundToInt(progress * 100f) + "%";

            yield return null;
        }

        // Cena carregada a 90%, agora barra enche até 100%
        loadingBar.value = 1f;
        if (loadingText != null)
            loadingText.text = "100%";

        yield return new WaitForSeconds(0.5f); // Pausa antes de entrar

        // Ativa a cena
        operation.allowSceneActivation = true;
    }
}
