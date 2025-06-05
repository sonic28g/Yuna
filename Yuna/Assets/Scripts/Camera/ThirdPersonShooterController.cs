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
    [SerializeField] private VignetteEffectHandler vignetteHandler;
    [SerializeField] private GameObject tessen;


    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController _thirdPersonController;
    private Animator _animator;
    private Outline[] kanzashiOutlines;
    private Outline[] enemyOutlines;

    private bool _hasAnimator;

    public bool isAttacking = false;
    public bool isCrouching = false;
    

    private void Awake()
    {
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
            vignetteHandler?.SetAimingEffect();

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
                    float projectileSpeed = 40f;
                    rb.linearVelocity = aimDir * projectileSpeed; // Define a velocidade na direção da mira
                }

                // Play the throw sound if the projectile has a WeaponObject component
                bool hasWeaponObject = projectileTransform.TryGetComponent<WeaponObject>(out var weaponObject);
                if (hasWeaponObject) weaponObject.PlayThrowSound();

                starterAssetsInputs.shoot = false;
            }
        }
        else if (!starterAssetsInputs.aim && isAttacking)
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("YourAnimationName") && stateInfo.normalizedTime < 1.0f)
            {
                tessen.SetActive(true);
            }
            else
            {
                tessen.SetActive(false);
            }

            _animator.SetTrigger("Attacking");
            starterAssetsInputs.shoot = false;
            isAttacking = false;
        }
        else
        {
            aimVirtualCamera.GetComponent<CinemachineVirtualCamera>().Priority = 0;
            vignetteHandler?.ResetVignette();

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

        if (starterAssetsInputs.scan && starterAssetsInputs.move == Vector2.zero)
        {
            vignetteHandler?.SetAimingEffect();
            SetKanzashiOutlinesActive(true);
        }
        else
        {
            vignetteHandler?.ResetVignette();
            SetKanzashiOutlinesActive(false);
            starterAssetsInputs.scan = false;

        }

        /*
        if(_animator.GetFloat("Speed") == _thirdPersonController.SprintSpeed && _animator.GetBool("Crouching"))
        {
            _animator.speed = 1.5f;
        }        
        else 
        {
            _animator.speed = 1f;
        }
        */
    }

    private void SetKanzashiOutlinesActive(bool active)
    {
        GameObject[] kanzashiObjects = GameObject.FindGameObjectsWithTag("Kanzashi");
        kanzashiOutlines = new Outline[kanzashiObjects.Length];
        for (int i = 0; i < kanzashiObjects.Length; i++)
        {
            kanzashiOutlines[i] = kanzashiObjects[i].GetComponent<Outline>();
        }

        if (kanzashiOutlines == null) return;

        foreach (Outline outline in kanzashiOutlines)
        {
            if (outline != null)
                outline.enabled = active;
        }
        
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        enemyOutlines = new Outline[enemyObjects.Length];
        for (int i = 0; i < enemyObjects.Length; i++)
        {
            enemyOutlines[i] = enemyObjects[i].GetComponent<Outline>();
        }

        if (enemyObjects == null) return;

        foreach (Outline outline in enemyOutlines)
        {
            if (outline != null)
                outline.enabled = active;
        }
    }

}
