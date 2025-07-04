using StarterAssets;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    [SerializeField] private Transform spawnProjectilePositionCrouched;

    [SerializeField] private VignetteEffectHandler vignetteHandler;
    [SerializeField] private GameObject tessen;

    [SerializeField] private float distance;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Image aimUI;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color transparentColor = new Color(1, 1, 1, 0.3f); // branco com transparência

    [SerializeField] private GameObject staminaBar;

    [SerializeField] private GameObject normalCamera;
    [SerializeField] private GameObject normalAimCamera;
    [SerializeField] private GameObject crouchedCamera;
    [SerializeField] private GameObject crouchedAimCamera;


    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController _thirdPersonController;
    private Animator _animator;
    private Outline[] kanzashiOutlines;
    private Outline[] enemyOutlines;
    private Outline[] guardsOutlines;
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

        if (starterAssetsInputs.aim && _hasAnimator && !starterAssetsInputs.sprint && WeaponSwitcher.instance.CurrentWeapon.weaponType == WeaponType.Kanzashi)
        {
            if(isCrouching)
                crouchedAimCamera.GetComponent<CinemachineVirtualCamera>().Priority = 20;
            else
                aimVirtualCamera.GetComponent<CinemachineVirtualCamera>().Priority = 20;

            _thirdPersonController.SetSensitivity(aimSensibility);

            _animator.SetBool("Aiming", starterAssetsInputs.aim);

            if (InventoryManager.instance.HasAmmo("Kanzashi"))
            {
                crossHair.SetActive(true);
                _animator.SetBool("HasAmmo", true);
            }
            else
            {
                _animator.SetBool("HasAmmo", false);
                starterAssetsInputs.aim = false;
                return;
            }

            transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);

            bool isPointingAtAlly = false;

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, distance, enemyMask))
            {
                isPointingAtAlly = hit.collider.CompareTag("Guard") || hit.collider.CompareTag("NPC");

                aimUI.color = isPointingAtAlly ? transparentColor : normalColor;
            }
            else
            {
                aimUI.color = normalColor;
            }

            if (starterAssetsInputs.shoot && !isPointingAtAlly && InventoryManager.instance.HasAmmo("Kanzashi"))
            {
                InventoryManager.instance.UseAmmo("Kanzashi");

                Transform projectileTransform;
                
                if(isCrouching)
                    projectileTransform = Instantiate(pfProjectile, spawnProjectilePositionCrouched.position, Quaternion.LookRotation(aimDir));
                else 
                    projectileTransform = Instantiate(pfProjectile, spawnProjectilePosition.position, Quaternion.LookRotation(aimDir));

                Rigidbody rb = projectileTransform.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    float projectileSpeed = 40f;
                    rb.linearVelocity = aimDir * projectileSpeed;
                }

                if (projectileTransform.TryGetComponent<WeaponObject>(out var weaponObject))
                    weaponObject.PlayThrowSound();

                starterAssetsInputs.shoot = false;
            }
        }
        else if (!starterAssetsInputs.aim && WeaponSwitcher.instance.CurrentWeapon.weaponType == WeaponType.Tessen && isAttacking)
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            _animator.SetTrigger("Attacking");
            starterAssetsInputs.shoot = false;
            isAttacking = false;
        }
        else
        {
            if(isCrouching)
                crouchedAimCamera.GetComponent<CinemachineVirtualCamera>().Priority = 0;
            else
                aimVirtualCamera.GetComponent<CinemachineVirtualCamera>().Priority = 0;
                
            _thirdPersonController.SetSensitivity(normalSensibility);
            crossHair.SetActive(false);
            _animator.SetBool("Aiming", false);
        }

        if (starterAssetsInputs.crouch)
        {
            isCrouching = !isCrouching; // alterna o estado
            _animator.SetBool("Crouching", isCrouching);

            starterAssetsInputs.crouch = false;
        }


        if (isCrouching)
        {
            normalCamera.SetActive(false);
            normalAimCamera.SetActive(false);

            crouchedCamera.SetActive(true);
            crouchedAimCamera.SetActive(true);
        }
        else
        {
            normalCamera.SetActive(true);
            normalAimCamera.SetActive(true);

            crouchedCamera.SetActive(false);
            crouchedAimCamera.SetActive(false);
        }

        if (starterAssetsInputs.scan && starterAssetsInputs.move == Vector2.zero)
        {
            vignetteHandler?.SetAimingEffect();
            SetOutlinesActive(true);
        }
        else
        {
            vignetteHandler?.ResetVignette();
            SetOutlinesActive(false);
            starterAssetsInputs.scan = false;

        }

        if (starterAssetsInputs.sprint && _thirdPersonController.canSprint)
            staminaBar.SetActive(true);
        else
            staminaBar.SetActive(false);
        


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

    private void SetOutlinesActive(bool active)
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
        

        GameObject[] guardsObjects = GameObject.FindGameObjectsWithTag("Guard");
        guardsOutlines = new Outline[guardsObjects.Length];
        for (int i = 0; i < guardsObjects.Length; i++)
        {
            guardsOutlines[i] = guardsObjects[i].GetComponent<Outline>();
        }

        if (guardsObjects == null) return;

        foreach (Outline outline in guardsOutlines)
        {
            if (outline != null)
                outline.enabled = active;
        }
    }

}
