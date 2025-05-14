using System;
using System.Linq;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [field: SerializeField] public int MaxHealth { get; private set; } = 100;
    public int CurrentHealth { get; private set; }

    public event Action OnDeath;
    public bool IsDead => CurrentHealth <= 0;

    [Header("Damage Settings")]
    [SerializeField] private int kanzashiDamage = 25;
    [SerializeField] private int kanzashiCriticalDamage = 50;
    [SerializeField] private int tessenDamage = 999;

    [Header("Critical Hit Areas")]
    [SerializeField] private Collider[] criticalColliders;


    private void Awake()
    {
        criticalColliders.ToList().ForEach(c => c.isTrigger = true);
        ResetHealth();
    }


    public void ResetHealth() => CurrentHealth = MaxHealth;

    public void Kill()
    {
        CurrentHealth = 0;
        OnDeath?.Invoke();
    }


    public void TakeDamage(WeaponType weapon, ContactPoint? contact = null)
    {
        if (IsDead) return;

        // Calculate damage based on weapon type
        int damage = CalculateDamage(weapon, contact);
        if (damage <= 0) return;

        // Apply damage & check for death
        CurrentHealth -= damage;
        if (IsDead) Kill();
    }

    private int CalculateDamage(WeaponType weapon, ContactPoint? contact) => weapon switch
    {
        WeaponType.Tessen => CalculateTessenDamage(),
        WeaponType.Kanzashi => CalculateKanzashiDamage(contact),
        _ => 0
    };


    private int CalculateTessenDamage() => tessenDamage;

    private int CalculateKanzashiDamage(ContactPoint? contact)
    {
        // If contact point is provided, check if it's a critical hit (and return critical damage)
        // Otherwise, return normal damage
        return contact.HasValue && IsCriticalHit(contact.Value.point)
            ? kanzashiCriticalDamage
            : kanzashiDamage;
    }

    private bool IsCriticalHit(Vector3 hitPoint) => criticalColliders.Any(c => c != null && c.bounds.Contains(hitPoint));
}
