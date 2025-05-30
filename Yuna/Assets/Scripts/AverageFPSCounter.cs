using UnityEngine;

public class AverageFPSCounter : MonoBehaviour
{
    private float totalFrames = 0f;
    private float totalTime = 0f;
    private float averageFPS = 0f;

    void Update()
    {
        totalFrames++;
        totalTime += Time.unscaledDeltaTime;

        if (totalTime > 0)
            averageFPS = totalFrames / totalTime;
    }

    void OnApplicationQuit()
    {
        Debug.Log("Média de FPS: " + averageFPS.ToString("F2"));
    }
}
