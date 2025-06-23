using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private BGMPlayer.BGMType _bgmType = BGMPlayer.BGMType.MainMenu;

    private void Start()
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
