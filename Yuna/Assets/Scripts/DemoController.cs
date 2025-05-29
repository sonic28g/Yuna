using UnityEngine;

public class DemoController : MonoBehaviour
{
    public GameObject finishDemoPanel;
    public MenuController menuController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            finishDemoPanel.SetActive(true);
            menuController.PauseGame();
            gameObject.SetActive(false);
        }
    }
}
