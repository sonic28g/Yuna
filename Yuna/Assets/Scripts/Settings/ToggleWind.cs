using UnityEngine;

public class ToggleWind : MonoBehaviour
{
    public Material[] materiaisDoShader;

    public void Toggle(bool ativarVento)
    {
        foreach (Material mat in materiaisDoShader)
        {
            if (mat != null)
            {
                if (ativarVento)
                {
                    mat.EnableKeyword("_CUSTOMWIND_ON");
                    mat.SetFloat("_CUSTOMWIND", 1f);
                }
                else
                {
                    mat.DisableKeyword("_CUSTOMWIND_ON");
                    mat.SetFloat("_CUSTOMWIND", 0f);
                }
            }
        }
    }
}