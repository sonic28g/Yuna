using System;
using System.IO;
using System.Linq;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private EnemyHealthData _healthData;
    private static readonly string ENEMY_HEALTH_FILE = "health.json";

    [Header("Health Settings")]
    [field: SerializeField] public int MaxHealth { get; private set; } = 100;
    public int? CurrentHealth { get; private set; }

    public event Action OnDeath;
    public bool IsDead => CurrentHealth.GetValueOrDefault(MaxHealth) <= 0;

    [Header("Damage Settings")]
    [SerializeField] private int kanzashiDamage = 25;
    [SerializeField] private int kanzashiCriticalDamage = 50;
    [SerializeField] private int tessenDamage = 999;

    [Header("Critical Hit Areas")]
    [SerializeField] private Collider[] criticalColliders;


    private void Awake() => criticalColliders.ToList().ForEach(c => c.isTrigger = true);


    public void ResetHealth(string enemyDir = null)
    {
        LoadHealthData(enemyDir);
        CurrentHealth = _healthData.CurrentHealth;
    }

    private void LoadHealthData(string enemyDir = null)
    {
        // Already loaded
        if (_healthData != null) return;

        // Set up the file path
        string filePath = Path.Combine(enemyDir ?? name, ENEMY_HEALTH_FILE);
        
        try
        {
            // Read the JSON file and deserialize it + Set the data variable
            string json = File.ReadAllText(filePath);
            _healthData = JsonUtility.FromJson<EnemyHealthData>(json) ?? throw new Exception($"Failed to parse health data from {filePath}");
        }
        catch (Exception e)
        {
            Debug.Log($"Failed to load health data for {name}: {e.Message}");

            // Initialize with default values if load fails
            _healthData = new EnemyHealthData
            {
                CurrentHealth = MaxHealth
            };
        }
    }


    public void SaveHealth(string enemyDir = null)
    {
        // Save data variable
        _healthData ??= new EnemyHealthData();
        _healthData.CurrentHealth = CurrentHealth.GetValueOrDefault(MaxHealth);

        // Set up the file path
        string filePath = Path.Combine(enemyDir ?? name, ENEMY_HEALTH_FILE);

        try
        {
            // Convert the data to JSON
            string json = JsonUtility.ToJson(_healthData);

            // Create the directory if it doesn't exist
            if (!Directory.Exists(enemyDir)) Directory.CreateDirectory(enemyDir);

            // Save the JSON to a file
            File.WriteAllText(filePath, json);
        }
        catch (Exception e)
        {
            Debug.Log($"Failed to save health data for {name}: {e.Message}");
        }
    }


    public void Kill()
    {
        // Check if the component is disabled
        if (!enabled) return;

        CurrentHealth = 0;
        OnDeath?.Invoke();

        Debug.Log($"{name} is dead.");
    }


    public void TakeDamage(WeaponType weapon, ContactPoint? contact = null)
    {
        // Check if the enemy is already dead or if the component is disabled
        if (!enabled || IsDead) return;

        // Calculate damage based on weapon type
        int damage = CalculateDamage(weapon, contact);
        if (damage <= 0) return;

        // Apply damage & check for death
        CurrentHealth -= damage;
        Debug.Log($"{name} took {damage} damage from {weapon}.\nCurrent Health: {CurrentHealth} / {MaxHealth}");

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


    [Serializable]
    private class EnemyHealthData
    {
        public int CurrentHealth;
    }
}
