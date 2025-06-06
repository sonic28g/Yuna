using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public ThirdPersonController TPController;
    public StarterAssetsInputs _inputs;

    public bool showingSettings = false;
    public bool showingPause = false;
    public bool showingKeybinds = false;
    public bool showingJournal = false;
    public bool showingOptions = false;
    public bool showingBriefing = false;

    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public GameObject keybindsMenu;
    public GameObject journalMenu;
    public GameObject optionsMenu;
    public GameObject briefingMenu;


    private void Update()
    {
        if (_inputs == null) return;

        if (_inputs.showMenu)
        {

            if (!showingPause && !showingSettings && !showingKeybinds && !showingOptions && !showingJournal && !showingBriefing)
            {
                showingPause = true;
                pauseMenu.SetActive(true);
                PauseGame();

            }
            else if (showingPause && !showingSettings && !showingKeybinds && !showingOptions && !showingJournal && !showingBriefing)
            {
                ResumeGame();
                pauseMenu.SetActive(false);
            }
            else if (!showingPause && showingSettings && !showingKeybinds && !showingOptions && !showingJournal && !showingBriefing)
            {
                settingsMenu.SetActive(false);
                showingSettings = false;
                showingOptions = true;
                optionsMenu.SetActive(true);
            }
            else if (!showingPause && !showingSettings && showingKeybinds && !showingOptions && !showingJournal && !showingBriefing)
            {
                keybindsMenu.SetActive(false);
                showingKeybinds = false;
                showingOptions = true;
                optionsMenu.SetActive(true);
            }
            else if (!showingPause && !showingSettings && !showingKeybinds && showingOptions && !showingJournal && !showingBriefing)
            {
                optionsMenu.SetActive(false);
                showingOptions = false;
                showingPause = true;
                pauseMenu.SetActive(true);
            }
            else if (!showingPause && !showingSettings && !showingKeybinds && !showingOptions && showingJournal && !showingBriefing)
            {
                journalMenu.SetActive(false);
                showingJournal = false;
                ResumeGame();
            }
            else if (!showingPause && !showingSettings && !showingKeybinds && !showingOptions && !showingJournal && showingBriefing)
            {
                briefingMenu.SetActive(false);
                showingBriefing = false;
                showingPause = true;
                pauseMenu.SetActive(true);
            }

            _inputs.showMenu = false;
        }

        if (_inputs.triggerJournal)
        {
            showingJournal = !showingJournal;
            journalMenu.SetActive(showingJournal);

            if (showingJournal == true)
            {
                DiaryManager.Instance.PlayOpenSound();
                PauseGame();
            }
            else
            {
                DiaryManager.Instance.PlayCloseSound();
                ResumeGame();
            }

            _inputs.triggerJournal = false;
        }
    }

    public void CloseGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ChangeToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        TPController.LockCameraPosition = true;
        Cursor.visible = true; // Esconde o cursor
        Cursor.lockState = CursorLockMode.None; // Tranca o cursor ao centro do ecrã     
    }

    public void ResumeGame()
    {
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

    public void ToggleOptionsShowing(bool option)
    {
        showingOptions = option;
    }

    public void ToggleJournalShowing(bool option)
    {
        showingJournal = option;
    }

    public void ToggleBriefingShowing(bool option)
    {
        showingBriefing = option;
    }
}
