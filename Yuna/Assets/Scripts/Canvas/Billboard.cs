using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCam;

    void Start()
    {
        // A câmara principal será controlada pela Cinemachine
        mainCam = Camera.main;
    }

    void LateUpdate()
    {
        if (mainCam == null) return;

        // Faz com que o aviso fique sempre virado para a câmara
        transform.forward = mainCam.transform.forward;
    }
}