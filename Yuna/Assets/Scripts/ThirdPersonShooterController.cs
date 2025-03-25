using StarterAssets;
using Unity.Cinemachine;
using UnityEngine;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] private GameObject aimVirtualCamera;
    [SerializeField] private float normalSensibility;
    [SerializeField] private float aimSensibility;
    [SerializeField] private GameObject crossHair;
    [SerializeField] private GameObject kanzashiPrefab;
    [SerializeField] private Transform shootPos;

    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController _thirdPersonController;
    private Animator _animator;
    private bool _hasAnimator;

    private void Awake() {
        _thirdPersonController = gameObject.GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }

    void Update()
    {   _hasAnimator = TryGetComponent(out _animator);

        if (starterAssetsInputs.aim && _hasAnimator)
        {
            aimVirtualCamera.transform.position = Camera.main.transform.position;
            aimVirtualCamera.transform.rotation = Camera.main.transform.rotation;

            aimVirtualCamera.GetComponent<CinemachineCamera>().Priority = 20;
            _thirdPersonController.SetSensitivity(aimSensibility);
            crossHair.SetActive(true);
            _animator.SetBool("Aiming", true);

            if (starterAssetsInputs.shoot)
            {
                Instantiate(kanzashiPrefab);
                starterAssetsInputs.shoot = false; // Garante que só dispara uma vez
            }
        }
        else
        {
            aimVirtualCamera.GetComponent<CinemachineCamera>().Priority = 0;
            _thirdPersonController.SetSensitivity(normalSensibility);
            crossHair.SetActive(false);
            _animator.SetBool("Aiming", false);

        }
    }
}
