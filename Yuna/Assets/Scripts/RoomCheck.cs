using UnityEngine;

public class RoomCheck : MonoBehaviour
{
    public bool isInRoom = false;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            isInRoom = true;

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isInRoom = false;

    }
}
