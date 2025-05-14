using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [field: SerializeField] public int MaxHealth { get; private set; } = 100;
    public int CurrentHealth { get; private set; }

    public event Action OnDeath;
    public bool IsDead => CurrentHealth <= 0;


    private void Awake()
    {
        ResetHealth();
    }


    public void ResetHealth() => CurrentHealth = MaxHealth;

    public void Kill()
    {
        CurrentHealth = 0;
        OnDeath?.Invoke();
    }
}
