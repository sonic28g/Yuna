using UnityEngine;

public class WeaponObject : InteractableObject
{
    public WeaponData weaponData; // Dados da arma (ScriptableObject)
    public int amount; // Quantidade de munição que este objeto dá

    private Rigidbody bulletRigidBody;

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

    private void Awake() {
        bulletRigidBody = GetComponent<Rigidbody>();
    }

    private void Start() {
        float speed = 20f;
        bulletRigidBody.linearVelocity = transform.forward * speed;
    }
}
