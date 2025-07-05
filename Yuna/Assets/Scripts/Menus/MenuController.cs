using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance { get; private set; }

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
    public GameObject helperPanel;

    private void Awake()
    {
        // Singleton Setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Garante que só uma instância permanece
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Opcional, caso queiras que persista entre cenas
    }

    private void Update()
    {
        if (_inputs == null) return;

        if (_inputs.showMenu)
        {
            HandleShowMenu();
            _inputs.showMenu = false;
        }

        if (_inputs.triggerJournal)
        {
            ToggleJournal();
            _inputs.triggerJournal = false;
        }

        if (helperPanel.activeSelf)
        {
            PauseGame();
        }
    }

    private void HandleShowMenu()
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
        else if (!showingPause && showingSettings)
        {
            settingsMenu.SetActive(false);
            showingSettings = false;
            showingOptions = true;
            optionsMenu.SetActive(true);
        }
        else if (!showingPause && showingKeybinds)
        {
            keybindsMenu.SetActive(false);
            showingKeybinds = false;
            showingOptions = true;
            optionsMenu.SetActive(true);
        }
        else if (showingOptions)
        {
            optionsMenu.SetActive(false);
            showingOptions = false;
            showingPause = true;
            pauseMenu.SetActive(true);
        }
        else if (showingJournal)
        {
            journalMenu.SetActive(false);
            showingJournal = false;
            ResumeGame();
        }
        else if (showingBriefing)
        {
            briefingMenu.SetActive(false);
            showingBriefing = false;
            showingPause = true;
            pauseMenu.SetActive(true);
        }
    }

    private void ToggleJournal()
    {
        showingJournal = !showingJournal;
        journalMenu.SetActive(showingJournal);

        if (showingJournal)
        {
            DiaryManager.Instance?.PlayOpenSound();
            PauseGame();
        }
        else
        {
            DiaryManager.Instance?.PlayCloseSound();
            ResumeGame();
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
        BGMPlayer.Instance?.Unmuffle(this);
        SceneManager.LoadScene(sceneName);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        if (TPController != null) TPController.LockCameraPosition = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        BGMPlayer.Instance?.Muffle(this);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        if (TPController != null) TPController.LockCameraPosition = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        showingPause = false;
        BGMPlayer.Instance?.Unmuffle(this);
    }

    // Métodos públicos para controlar flags manualmente
    public void ToggleSettingsShowing(bool option) => showingSettings = option;
    public void ToggleKeybindsShowing(bool option) => showingKeybinds = option;
    public void TogglePauseShowing(bool option) => showingPause = option;
    public void ToggleOptionsShowing(bool option) => showingOptions = option;
    public void ToggleJournalShowing(bool option) => showingJournal = option;
    public void ToggleBriefingShowing(bool option) => showingBriefing = option;
}
