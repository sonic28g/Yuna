using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    private Dictionary<string, int> ammoDictionary = new Dictionary<string, int>();

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void AddAmmo(string weaponName, int amount)
    {
        if (!ammoDictionary.ContainsKey(weaponName))
            ammoDictionary[weaponName] = 0;

        ammoDictionary[weaponName] += amount;

        // Atualiza UI da munição
        UIManager.instance.UpdateAmmoUI(weaponName, ammoDictionary[weaponName]);
    }

    public bool HasAmmo(string weaponName)
    {
        return ammoDictionary.ContainsKey(weaponName) && ammoDictionary[weaponName] > 0;
    }

    public void UseAmmo(string weaponName)
    {
        if (HasAmmo(weaponName))
        {
            ammoDictionary[weaponName]--;
            UIManager.instance.UpdateAmmoUI(weaponName, ammoDictionary[weaponName]);
        }
    }

    public int GetAmmo(string weaponName)
    {
        return ammoDictionary.ContainsKey(weaponName) ? ammoDictionary[weaponName] : 0;
    }
}
