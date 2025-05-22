using StarterAssets;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] private GameObject aimVirtualCamera;
    [SerializeField] private float normalSensibility;
    [SerializeField] private float aimSensibility;
    [SerializeField] private GameObject crossHair;
    [SerializeField] private GameObject kanzashiPrefab;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform pfProjectile;
    [SerializeField] private Transform spawnProjectilePosition;


    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController _thirdPersonController;
    private Animator _animator;
    private bool _hasAnimator;

    private bool nearEnemy = false;
    private bool isCrouching = false;

    private void Awake() {
        _thirdPersonController = gameObject.GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }

    void Update()
    {   
        _hasAnimator = TryGetComponent(out _animator);

        // Obtém a direção da mira (da câmara)
        Vector3 aimDir = Camera.main.transform.forward;

        if (starterAssetsInputs.aim && _hasAnimator && !starterAssetsInputs.sprint)
        {
            aimVirtualCamera.GetComponent<CinemachineVirtualCamera>().Priority = 20;
            _thirdPersonController.SetSensitivity(aimSensibility);
            
            crossHair.SetActive(true);
            _animator.SetBool("Aiming", starterAssetsInputs.aim);

            transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);

            if (starterAssetsInputs.shoot && InventoryManager.instance.HasAmmo("Kanzashi"))
            {
                InventoryManager.instance.UseAmmo("Kanzashi");
                // Instancia o projetil na posição correta e na direção da mira
                Transform projectileTransform = Instantiate(pfProjectile, spawnProjectilePosition.position, Quaternion.LookRotation(aimDir));

                // Aplica velocidade ao projetil, se tiver um Rigidbody
                Rigidbody rb = projectileTransform.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    float projectileSpeed = 20f;
                    rb.linearVelocity = aimDir * projectileSpeed; // Define a velocidade na direção da mira
                }

                starterAssetsInputs.shoot = false;
            }
        }
        else if (!starterAssetsInputs.aim && starterAssetsInputs.shoot && nearEnemy)
        {
            _animator.SetTrigger("Attacking");
            starterAssetsInputs.shoot = false;

        }
        else
        {
            aimVirtualCamera.GetComponent<CinemachineVirtualCamera>().Priority = 0;
            _thirdPersonController.SetSensitivity(normalSensibility);
            crossHair.SetActive(false);
            _animator.SetBool("Aiming", false);
        }

    if (starterAssetsInputs.crouch)
    {
        isCrouching = !isCrouching; // alterna o estado
        _animator.SetBool("Crouching", isCrouching);
        starterAssetsInputs.crouch = false; // impede múltiplos toggles no mesmo frame
    }

    }

}
