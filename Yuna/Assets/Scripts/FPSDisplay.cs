using UnityEngine;
using TMPro;
using System;

[ExecuteAlways]
public class FPSDisplay : MonoBehaviour
{
    public float fps { get; private set; }      // FPS médio do intervalo atual
    public float frameMS { get; private set; }  // MS por frame no intervalo

    GUIStyle style = new GUIStyle();
    public int size = 16;

    [Space]
    public Vector2 position = new Vector2(16.0f, 16.0f);

    public enum Alignment { Left, Right }
    public Alignment alignment = Alignment.Left;

    [Space]
    public Color colour = Color.green;
    public float updateInterval = 0.5f;

    float elapsedIntervalTime;
    int intervalFrameCount;

    [Space]
    [Tooltip("Optional. Will render using GUI if not assigned.")]
    public TextMeshProUGUI textMesh;

    // Nova parte para média dos fps
    private float totalFPS;
    private int intervalCount;
    private float averageFPS;

    public float GetIntervalFPS() => intervalFrameCount / elapsedIntervalTime;
    public float GetIntervalFrameMS() => (elapsedIntervalTime * 1000.0f) / intervalFrameCount;

    void Update()
    {
        intervalFrameCount++;
        elapsedIntervalTime += Time.unscaledDeltaTime;

        if (elapsedIntervalTime >= updateInterval)
        {
            fps = (float)Math.Round(GetIntervalFPS(), 2);
            frameMS = (float)Math.Round(GetIntervalFrameMS(), 2);

            // Acumula os valores para fazer a média
            totalFPS += fps;
            intervalCount++;
            averageFPS = totalFPS / intervalCount;

            intervalFrameCount = 0;
            elapsedIntervalTime = 0.0f;
        }

        if (textMesh)
        {
            textMesh.text = GetFPSText();
        }
        else
        {
            style.fontSize = size;
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = colour;
        }
    }

    string GetFPSText()
    {
        return $"FPS: {fps:0.00} ({frameMS:0.00} ms)\nMédia FPS: {averageFPS:0.00}";
    }

    void OnGUI()
    {
        if (!textMesh)
        {
            string fpsText = GetFPSText();
            float x = position.x;

            if (alignment == Alignment.Right)
                x = Screen.width - x - style.CalcSize(new GUIContent(fpsText)).x;

            GUI.Label(new Rect(x, position.y, 200, 100), fpsText, style);
        }
    }

    void OnApplicationQuit()
    {
        Debug.Log("Média de FPS (quando o jogo parou): " + averageFPS.ToString("F2"));
    }
}
