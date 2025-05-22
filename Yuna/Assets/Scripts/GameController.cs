using UnityEngine;
using UnityEngine.UI;
using StarterAssets;

public class GameController : MonoBehaviour
{
    public GameObject pauseMenu; 
    public ThirdPersonController TPController;

    public bool isSettingsShowing = false;
    void Awake()
    {
        //GameSettings settings = GameSettings.LoadFromFile();
        //SettingsManager.SetQualityLevel(settings.qualityLevel);
    }

    void Start()
    {
        Cursor.visible = false; // Esconde o cursor
        Cursor.lockState = CursorLockMode.Locked; // Tranca o cursor ao centro do ecrã
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape) && !isSettingsShowing)
        {
            pauseMenu.SetActive(true);
            PauseGame();
            Cursor.visible = true; // Esconde o cursor
            Cursor.lockState = CursorLockMode.None; // Tranca o cursor ao centro do ecrã
        }
    }

    public void PauseGame(){
        Time.timeScale = 0;
        TPController.LockCameraPosition = true;
        // parar camera
        
    }

    public void ResumeGame(){
        Time.timeScale = 1;
        TPController.LockCameraPosition = false;
        Cursor.visible = false; // Esconde o cursor
        Cursor.lockState = CursorLockMode.Locked; // Tranca o cursor ao centro do ecrã
    }

    public void ToggleSettingsShowing(bool option)
    {
        isSettingsShowing = option;
    }
}
