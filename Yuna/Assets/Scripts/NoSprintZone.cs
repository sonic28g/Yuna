using StarterAssets;
using UnityEngine;

public class NoSprintZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var controller = other.GetComponent<ThirdPersonController>();
        if (controller != null)
        {
            controller.canSprint = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var controller = other.GetComponent<ThirdPersonController>();
        if (controller != null)
        {
            controller.canSprint = true;
        }
    }
}
