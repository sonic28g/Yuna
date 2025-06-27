using UnityEngine;

public class KanzashiHairController : MonoBehaviour
{
    private int currentAmmo;

    [SerializeField] GameObject kanzashi1;
    [SerializeField] GameObject kanzashi2;
    [SerializeField] GameObject kanzashi3;
    [SerializeField] GameObject kanzashi4;
    [SerializeField] GameObject kanzashi5;

    void Update()
    {
        currentAmmo = InventoryManager.instance.GetAmmo("Kanzashi");

        kanzashi1.SetActive(currentAmmo >= 1);
        kanzashi2.SetActive(currentAmmo >= 2);
        kanzashi3.SetActive(currentAmmo >= 3);
        kanzashi4.SetActive(currentAmmo >= 4);
        kanzashi5.SetActive(currentAmmo >= 5);
    }
}
