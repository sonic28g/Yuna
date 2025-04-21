using UnityEngine;

public class ToggleWind : MonoBehaviour
{

    public Material materialDoShader;

    public void Toggle(bool ativarVento)
    {
        if (ativarVento)
        {
            materialDoShader.EnableKeyword("_CUSTOMWIND_ON");
            materialDoShader.SetFloat("_CUSTOMWIND", 1f);
        }
        else
        {
            materialDoShader.DisableKeyword("_CUSTOMWIND_ON");
            materialDoShader.SetFloat("_CUSTOMWIND", 0f);
        }
    }
}
