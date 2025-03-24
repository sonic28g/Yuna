using StarterAssets;
using Unity.Cinemachine;
using UnityEngine;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] private GameObject aimVirtualCamera;
    [SerializeField] private float normalSensibility;
    [SerializeField] private float aimSensibility;
    [SerializeField] private GameObject crossHair;

    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController thirdPersonController;

    private void Awake() {
        thirdPersonController = gameObject.GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }

    void Update()
    {
        if (starterAssetsInputs.aim)
        {
            // Copia posição e rotação da câmara principal
            aimVirtualCamera.transform.position = Camera.main.transform.position;
            aimVirtualCamera.transform.rotation = Camera.main.transform.rotation;

            aimVirtualCamera.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensibility);
            crossHair.SetActive(true);
        }
        else
        {
            aimVirtualCamera.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensibility);
            crossHair.SetActive(false);
        }
    }
}
