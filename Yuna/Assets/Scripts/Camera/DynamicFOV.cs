using UnityEngine;
using Unity.Cinemachine;

public class DynamicFOV : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera; // A tua Cinemachine Virtual Camera
    public Transform player; // O personagem ou alvo que a câmara segue
    public float runFOV = 70f; // FOV quando o personagem está a correr
    public float walkFOV = 60f; // FOV quando o personagem está a andar
    public float fovChangeSpeed = 5f; // A velocidade com que o FOV vai mudar
    public float minSpeedThreshold = 0.1f; // Limite para a velocidade mínima do andar (para evitar mudanças bruscas)

    private Vector3 lastPosition;
    private float currentSpeed;
    private float targetFOV;

    private float smoothSpeed;

    void Start()
    {
        lastPosition = player.position;
        targetFOV = walkFOV;
        smoothSpeed = 0f;
    }

    void Update()
    {
        // Calcula a distância percorrida desde o último frame
        float distanceMoved = Vector3.Distance(player.position, lastPosition);

        // Atualiza a velocidade com base na distância percorrida
        currentSpeed = distanceMoved / Time.deltaTime;

        // Verifica se a velocidade do personagem está acima do limite mínimo para andar
        if (currentSpeed > minSpeedThreshold)
        {
            // Se a velocidade for maior que o limite, o personagem está a correr
            if (currentSpeed > 2f) // Ajusta esse valor conforme a velocidade de corrida do jogador
            {
                targetFOV = runFOV;
            }
            else
            {
                targetFOV = walkFOV;
            }
        }

        // Faz o Lerp do FOV de forma mais suave
        smoothSpeed = Mathf.Lerp(smoothSpeed, currentSpeed, Time.deltaTime * fovChangeSpeed);
        float fov = Mathf.Lerp(virtualCamera.m_Lens.FieldOfView, targetFOV, smoothSpeed * Time.deltaTime);
        virtualCamera.m_Lens.FieldOfView = fov;

        // Atualiza a última posição do jogador
        lastPosition = player.position;
    }
}
