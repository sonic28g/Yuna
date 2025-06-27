using UnityEngine;

public class KanzashiHairController : MonoBehaviour
{
    [SerializeField] int maxKanzashi;

    private int currentAmmo;

    void Update()
    {
        currentAmmo = InventoryManager.instance.GetAmmo("Kanzashi");

        switch (currentAmmo)
        {
            case 0:
                print("0 kanzashi on hair");
                break;
            case 1:
                print("1 kanzashi on hair");
                break;
            case 2:
                print("2 kanzashi on hair");
                break;

        }
    }
}
