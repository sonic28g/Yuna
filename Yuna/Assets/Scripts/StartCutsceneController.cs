using UnityEngine;
using UnityEngine.SceneManagement;

public class StartCutsceneController : MonoBehaviour
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
}
