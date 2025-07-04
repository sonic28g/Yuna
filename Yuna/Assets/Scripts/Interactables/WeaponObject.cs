using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponObject : InteractableObject
{
    private readonly HashSet<EnemyHealth> _hitEnemies = new();

    [Header("Weapon Object Settings")]
    public WeaponData weaponData; // Dados da arma (ScriptableObject)
    public int amount; // Quantidade de munição que este objeto dá

    [SerializeField] private WeaponType _weaponType;

    [Header("Sound Settings")]
    [SerializeField, Tooltip("Layers that in which a sound will be emitted")]
    private LayerMask _soundMakerMask;
    private static readonly string WALL_TAG = "Wall";
    [SerializeField] private GameObject _soundEmitterPrefab;

    [SerializeField] private AudioClip[] _startClips;
    private PlayerInteraction playerInteraction;



    private void Awake()
    {
        playerInteraction = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>();
    }

    public void PlayThrowSound()
    {
        bool hasSource = TryGetComponent(out AudioSource audioSource);
        if (!hasSource || _startClips.Length == 0) return;

        int randomIndex = Random.Range(0, _startClips.Length);
        audioSource.clip = _startClips[randomIndex];
        audioSource.Play();
    }


    public override void Interact()
    {
        // Adiciona munição ao inventário
        InventoryManager.instance.AddAmmo(weaponData.weaponName, amount);

        UIManager.instance.AmmoNumberEffect();

        playerInteraction.ClearNearbyObject();

        // Destroi o objeto do mundo
        Destroy(gameObject);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision == null || collision.gameObject == null) return;

        TryEmitSound(collision);
        TryDamageEnemy(collision);
    }

    private void TryEmitSound(Collision collision)
    {
        if (_soundEmitterPrefab == null) return;

        bool isWall = collision.gameObject.CompareTag(WALL_TAG);

        // Check if the collision layer is in the mask
        int collisionLayer = collision.gameObject.layer;
        int collisionMask = 1 << collisionLayer;

        bool notInMask = (_soundMakerMask & collisionMask) == 0;
        if (!isWall && notInMask) return;

        // Create the sound emitter at the contact point
        ContactPoint contactPoint = collision.GetContact(0);
        Instantiate(_soundEmitterPrefab, contactPoint.point, Quaternion.identity);
    }

    private void TryDamageEnemy(Collision collision)
    {
        // Check if the collided object has an EnemyHealth component
        bool hasEnemyHealth = collision.gameObject.TryGetComponent(out EnemyHealth enemy);
        if (!hasEnemyHealth) return;

        // Check if the enemy is already hit
        bool alreadyOnHits = _hitEnemies.Contains(enemy);
        if (alreadyOnHits) return;

        // Try to damage the enemy
        ContactPoint contactPoint = collision.GetContact(0);
        enemy.TakeDamage(_weaponType, contactPoint);

        // Add the enemy to the hit list
        _hitEnemies.Add(enemy);
    }
}


public enum WeaponType
{
    Kanzashi,
    Tessen
}
