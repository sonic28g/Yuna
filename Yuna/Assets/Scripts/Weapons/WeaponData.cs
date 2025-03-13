using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapon System/Weapon")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public GameObject projectilePrefab; // Prefab da munição (se for uma arma de ataque à distância)
    public int maxAmmo; // Munição máxima (0 se for infinita)
    public bool hasInfiniteAmmo;
    public float attackCooldown; // Tempo entre ataques
}
