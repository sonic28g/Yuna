using UnityEngine;

public class EnemyIndicator : MonoBehaviour
{
    public Transform enemy; // Referência ao inimigo
    public RectTransform indicatorUI; // O ponto de exclamação
    public Camera mainCamera;
    public float edgeBuffer = 50f; // Distância da margem do ecrã

    void Update()
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(enemy.position + Vector3.up * 2f); // Offset para cima da cabeça

        bool isOffScreen = screenPos.z < 0 || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height;

        if (isOffScreen)
        {
            // Colocar na margem
            Vector3 cappedScreenPos = screenPos;

            cappedScreenPos.x = Mathf.Clamp(screenPos.x, edgeBuffer, Screen.width - edgeBuffer);
            cappedScreenPos.y = Mathf.Clamp(screenPos.y, edgeBuffer, Screen.height - edgeBuffer);

            // Se estiver atrás da câmara, inverter a direção horizontal
            if (screenPos.z < 0)
                cappedScreenPos.x = Screen.width - cappedScreenPos.x;

            indicatorUI.position = cappedScreenPos;
        }
        else
        {
            // Colocar sobre a cabeça do inimigo
            indicatorUI.position = screenPos;
        }
    }
}