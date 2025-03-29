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

    private void Awake() {
        _thirdPersonController = gameObject.GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }

    void Update()
    {   _hasAnimator = TryGetComponent(out _animator);

        Vector3 mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        if(Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            mouseWorldPosition = raycastHit.point;
        }

        if (starterAssetsInputs.aim && _hasAnimator)
        {
            aimVirtualCamera.GetComponent<CinemachineVirtualCamera>().Priority = 20;
            _thirdPersonController.SetSensitivity(aimSensibility);
            
            crossHair.SetActive(true);
            _animator.SetBool("Aiming", true);

            // Faz a personagem rodar na mesma direção que a câmara
            transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);

            if (starterAssetsInputs.shoot)
            {
                Vector3 aimDir = (mouseWorldPosition - spawnProjectilePosition.position).normalized;

                Transform projectileTransform = Instantiate(pfProjectile, spawnProjectilePosition.position, Quaternion.identity);
                
                // Ajusta a rotação do projetil para seguir a direção do tiro
                projectileTransform.forward = aimDir;

                // Aplica velocidade ao Rigidbody do projetil (se tiver um)
                Rigidbody rb = projectileTransform.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    float projectileSpeed = 20f;
                    rb.linearVelocity = aimDir * projectileSpeed;
                }

                starterAssetsInputs.shoot = false;
            }
        }
        else
        {
            aimVirtualCamera.GetComponent<CinemachineVirtualCamera>().Priority = 0;
            _thirdPersonController.SetSensitivity(normalSensibility);
            crossHair.SetActive(false);
            _animator.SetBool("Aiming", false);
        }

    }
}
