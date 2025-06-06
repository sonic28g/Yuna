using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Camera mainCamera;

    void LateUpdate()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        transform.LookAt(transform.position + mainCamera.transform.forward);
    }
}