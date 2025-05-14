using UnityEngine;

public class WeaponObject : InteractableObject
{
    [Header("Weapon Object Settings")]
    public WeaponData weaponData; // Dados da arma (ScriptableObject)
    public int amount; // Quantidade de munição que este objeto dá

    [Header("Sound Settings")]
    [SerializeField, Tooltip("Layers that in which a sound will be emitted")]
    private LayerMask _soundMakerMask;
    [SerializeField] private GameObject _soundEmitterPrefab;


    public override void Interact()
    {
        Debug.Log("Pegaste a arma: " + weaponData.weaponName);
        
        // Adiciona munição ao inventário
        InventoryManager.instance.AddAmmo(weaponData.weaponName, amount);
        
        // Mostra mensagem na UI
        UIManager.instance.ShowPickupText(weaponData.weaponName);

        // Destroi o objeto do mundo
        Destroy(gameObject);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision == null || collision.gameObject == null) return;

        TryEmitSound(collision);
    }

    private void TryEmitSound(Collision collision)
    {
        if (_soundEmitterPrefab == null) return;

        // Check if the collision layer is in the mask
        int collisionLayer = collision.gameObject.layer;
        int collisionMask = 1 << collisionLayer;

        bool notInMask = (_soundMakerMask & collisionMask) == 0;
        if (notInMask) return;

        // Create the sound emitter at the contact point
        ContactPoint contactPoint = collision.GetContact(0);
        Instantiate(_soundEmitterPrefab, contactPoint.point, Quaternion.identity);
    }
}


public enum WeaponType
{
    Kanzashi,
    Tessen
}
