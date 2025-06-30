using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private BGMType _bgmType = BGMType.MainMenu;

    private void OnEnable()
    {
        if (BGMPlayer.Instance != null) BGMPlayer.Instance.Play(_bgmType);
    }

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void NewGame(string sceneName)
    {
        TryDelete("Player");
        TryDelete("Dialogue");
        TryDelete("Enemies");
        TryDelete("NPCs");
        TryDelete("Inspectable");
        ChangeToScene(sceneName);
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


    private void TryDelete(string dirName)
    {
        try
        {
            string path = $"{Application.persistentDataPath}/{dirName}";
            Directory.Delete(path, true);
        }
        catch (System.Exception e)
        {
            Debug.Log($"Failed to delete {dirName}: {e.Message}");
        }
    }

    public bool HaveSavedGame()
    {
        try
        {
            string playerDir = $"{Application.persistentDataPath}/Player";
            string dialogueDir = $"{Application.persistentDataPath}/Dialogue";
            string enemiesDir = $"{Application.persistentDataPath}/Enemies";
            string npcsDir = $"{Application.persistentDataPath}/NPCs";

            // Check if any of the directories exist
            return Directory.Exists(playerDir) ||
                   Directory.Exists(dialogueDir) ||
                   Directory.Exists(enemiesDir) ||
                   Directory.Exists(npcsDir);
        }
        catch (System.Exception e)
        {
            Debug.Log($"Error checking saved game: {e.Message}");
            return false;
        }
    }
}
