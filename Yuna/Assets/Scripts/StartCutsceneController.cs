using UnityEngine;
using UnityEngine.SceneManagement;

public class StartCutsceneController : MonoBehaviour
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
}
