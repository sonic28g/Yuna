using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private StarterAssetsInputs starterAssetsInputs;

    private bool isOpen;
    public ThirdPersonController TPController;
    public StarterAssetsInputs _inputs;

    public bool showingSettings = false; 
    public bool showingPause = false; 
    public bool showingKeybinds = false;

    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public GameObject keybindsMenu;


    private void Awake() 
    {
        isOpen = false;
    }

    private void Update() 
    {
        if(_inputs.showMenu)
        {
            if(!showingPause && !showingSettings && !showingKeybinds)
            {
                showingPause = true;
                pauseMenu.SetActive(true);
                PauseGame();
                Cursor.visible = true; // Esconde o cursor
                Cursor.lockState = CursorLockMode.None; // Tranca o cursor ao centro do ecrã
            }
            else if(showingPause && !showingSettings && !showingKeybinds)
            {
                ResumeGame();
                pauseMenu.SetActive(false);
            }
            else if(!showingPause && showingSettings && !showingKeybinds)
            {
                settingsMenu.SetActive(false);
                showingSettings = false;
                showingPause = true;
                pauseMenu.SetActive(true);
            }
            else if(!showingPause && !showingSettings && showingKeybinds)
            {
                keybindsMenu.SetActive(false);
                showingKeybinds = false;
                showingPause = true;
                pauseMenu.SetActive(true);
            }

            _inputs.showMenu = false;

        }
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void ChangeToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
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
        showingPause = false;
    }

    public void ToggleSettingsShowing(bool option)
    {
        showingSettings = option;
    }

    
    public void ToggleKeybindsShowing(bool option)
    {
        showingKeybinds = option;
    }

    public void TogglePauseShowing(bool option)
    {
        showingPause = option;
    }
}
