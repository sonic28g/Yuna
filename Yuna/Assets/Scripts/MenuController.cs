using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private StarterAssetsInputs starterAssetsInputs;

    private bool isOpen;
    //public GameObject menu;

    private void Awake() 
    {
        isOpen = false;
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }

    private void Update() 
    {
        if (starterAssetsInputs.showMenu)
        {
            OpenMenu();
            starterAssetsInputs.showMenu = false;
        }
    }

    public void OpenMenu()
    {
        isOpen = !isOpen;
        //menu.SetActive(isOpen);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void ChangeToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ToggleMouse()
    {
        
    }
}
