using UnityEngine;

public class Attack : InteractableObject
{
    private ThirdPersonShooterController thirdPersonShooterController;

    [Header("Sound Settings")]
    [SerializeField] private AudioClip[] _tessenClips;
    [SerializeField] private AudioSource _audioSource;


    private void Start()
    {
        thirdPersonShooterController = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonShooterController>();
    }


    public override void Interact()
    {
        if (WeaponSwitcher.instance.CurrentWeapon.weaponType != WeaponType.Tessen) return;

        thirdPersonShooterController.isAttacking = true;
        PlayTessenSound();

        EnemyHealth enemyHealth = gameObject.GetComponentInParent<EnemyHealth>();
        if (enemyHealth != null) enemyHealth.TakeDamage(WeaponType.Tessen);

        gameObject.SetActive(false);
    }

    private void PlayTessenSound()
    {
        if (_audioSource == null || _tessenClips.Length == 0) return;

        int randomIndex = Random.Range(0, _tessenClips.Length);
        _audioSource.PlayOneShot(_tessenClips[randomIndex]);
    }
}
