using UnityEngine;

public class ToggleFPSDisplay : MonoBehaviour
{
    public GameObject fpsDisplay;
    private bool show = false;

    public void ToggleFPS()
    {
        if (fpsDisplay != null)
        {
            show = !show;
            fpsDisplay.SetActive(show);
        }
    }
}