using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private BGMType _bgmType = BGMType.MainMenu;

    private void OnEnable()
    {
        if (BGMPlayer.Instance != null) BGMPlayer.Instance.Play(_bgmType);
    }


    public void ChangeToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void CloseGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
