using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    private readonly Dictionary<string, int> ammoDictionary = new();

    private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _takeClips;


    private void Awake()
    {
        if (instance == null) instance = this;
        _audioSource = GetComponent<AudioSource>();
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
}
