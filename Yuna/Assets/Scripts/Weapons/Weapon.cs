using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponData weaponData; // Referência ao Scriptable Object da arma
    public Transform firePoint; // Ponto de disparo
    private float lastAttackTime;

    public void Attack()
    {
        if (Time.time - lastAttackTime < weaponData.attackCooldown) return; // Controla tempo entre ataques

        if (!weaponData.hasInfiniteAmmo)
        {
            if (!InventoryManager.instance.HasAmmo(weaponData.weaponName))
            {
                UIManager.instance.ShowInteractionText("You have no kanzashis");
                return;
            }
            InventoryManager.instance.UseAmmo(weaponData.weaponName);
        }

        lastAttackTime = Time.time;
    }
}
