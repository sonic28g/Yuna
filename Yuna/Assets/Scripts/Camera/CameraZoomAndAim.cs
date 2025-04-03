using Unity.Cinemachine;
using UnityEngine;

public class CameraZoomAndAim : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;  // A tua Cinemachine Virtual Camera
    private CinemachineTransposer transposer;  // O componente Transposer da câmara

    public Transform aimTarget;  // A posição da mira

    public float normalOffsetZ = -5f;  // Distância normal
    public float aimOffsetZ = -3f;     // Distância de zoom (ao entrar em aim)
    public float transitionSpeed = 10f;  // A velocidade de transição do offset

    private Vector3 targetOffset;  // O offset de destino

    void Start()
    {
        // Obter o componente CinemachineTransposer
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetOffset = new Vector3(0, 1, normalOffsetZ);  // Offset inicial
        transposer.m_FollowOffset = targetOffset;  // Definir o offset inicial
    }

    void Update()
    {
        // Verifica se está no modo de aim (exemplo usando o botão direito do rato)
        bool isAiming = Input.GetMouseButton(1);  // Usando o botão direito do rato para mirar

        // Ajuste suave do offset Z
        if (isAiming)
        {
            targetOffset.z = Mathf.Lerp(targetOffset.z, aimOffsetZ, Time.deltaTime * transitionSpeed);
        }
        else
        {
            targetOffset.z = Mathf.Lerp(targetOffset.z, normalOffsetZ, Time.deltaTime * transitionSpeed);
        }

        // Atualiza o offset da câmara (sem mover a mira)
        transposer.m_FollowOffset = targetOffset;

        // Manter a câmara a olhar para a mira
        if (isAiming)
        {
            virtualCamera.transform.LookAt(aimTarget);  // Faz a câmara olhar para a mira
        }
    }
}

