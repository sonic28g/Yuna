using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    private readonly Dictionary<string, int> ammoDictionary = new();

    private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _takeClips;

    private string _playerDir;
    private string InventoryFilePath => $"{_playerDir}/inventory.json";
    private InventoryData _invData;


    private void Awake()
    {
        if (instance == null) instance = this;

        _audioSource = GetComponent<AudioSource>();

        _playerDir = $"{Application.persistentDataPath}/Player";
        ResetInventory();
    }

    private void Start()
    {
        // Initialize ammo UI with loaded data
        if (UIManager.instance == null) return;
        foreach (var kvp in ammoDictionary) UIManager.instance.UpdateAmmoUI(kvp.Key, kvp.Value);
    }


    private void PlayTakeSound()
    {
        if (_audioSource == null || _takeClips.Length == 0) return;

        int randomIndex = Random.Range(0, _takeClips.Length);
        _audioSource.clip = _takeClips[randomIndex];
        _audioSource.Play();
    }


    public void AddAmmo(string weaponName, int amount)
    {
        if (!ammoDictionary.ContainsKey(weaponName))
            ammoDictionary[weaponName] = 0;

        ammoDictionary[weaponName] += amount;

        // Atualiza UI da munição
        UIManager.instance.UpdateAmmoUI(weaponName, ammoDictionary[weaponName]);

        PlayTakeSound();
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


    public void ResetInventory()
    {
        LoadFromFile();

        // Reset ammo counts to zero for all weapons
        ammoDictionary.Keys.ToList().ForEach(key => ammoDictionary[key] = 0);

        // Populate ammoDictionary with loaded data
        if (_invData != null && _invData.InventoryEntries != null)
            foreach (var entry in _invData.InventoryEntries)
                if (!string.IsNullOrEmpty(entry.WeaponName)) ammoDictionary[entry.WeaponName] = entry.AmmoCount;

        // Update UI
        if (UIManager.instance == null) return;
        foreach (var kvp in ammoDictionary) UIManager.instance.UpdateAmmoUI(kvp.Key, kvp.Value);
    }

    private void LoadFromFile()
    {
        // Already loaded
        if (_invData != null) return;

        try
        {
            // Read the JSON file and deserialize it + Set the data variable
            string json = File.ReadAllText(InventoryFilePath);
            _invData = JsonUtility.FromJson<InventoryData>(json) ?? throw new System.Exception($"Failed to parse inventory data from {InventoryFilePath}");
        }
        catch (System.Exception e)
        {
            Debug.Log($"Failed to load inventory data for {name}: {e.Message}");

            // Initialize with default values if load fails
            _invData = new();
        }
    }

    public void SaveInventory()
    {
        // Save data variable
        _invData ??= new();
        _invData.InventoryEntries = ammoDictionary
            .Select(kvp => new InventoryData.InvDataEntry {
                WeaponName = kvp.Key,
                AmmoCount = kvp.Value
            })
            .ToArray();

        try
        {
            // Convert the data to JSON
            string json = JsonUtility.ToJson(_invData);

            // Create the directory if it doesn't exist
            if (!Directory.Exists(_playerDir)) Directory.CreateDirectory(_playerDir);

            // Save the JSON to a file
            File.WriteAllText(InventoryFilePath, json);
        }
        catch (System.Exception e)
        {
            Debug.Log($"Failed to save inventory data for {name}: {e.Message}");
        }
    }


    [System.Serializable]
    private class InventoryData
    {
        public InvDataEntry[] InventoryEntries;

        [System.Serializable]
        public class InvDataEntry
        {
            public string WeaponName;
            public int AmmoCount;
        }
    }
}
