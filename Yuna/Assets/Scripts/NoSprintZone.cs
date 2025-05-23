using StarterAssets;
using UnityEngine;
using Unity.Cinemachine;

public class NoSprintZone : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float newDistance = 3.5f;
    public float originalDistance = 4.5f;

    private void OnTriggerEnter(Collider other)
    {
        var controller = other.GetComponent<ThirdPersonController>();
        if (controller != null)
        {
            controller.canSprint = false;

            var transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            if (transposer != null)
            {
                transposer.m_CameraDistance = newDistance;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var controller = other.GetComponent<ThirdPersonController>();
        if (controller != null)
        {
            controller.canSprint = true;

            var transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            if (transposer != null)
            {
                transposer.m_CameraDistance = originalDistance;
            }
        }
    }
}
