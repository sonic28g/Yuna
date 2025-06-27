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

        switch (currentAmmo)
        {
            case 0:
                kanzashi1.SetActive(false);
                kanzashi2.SetActive(false);
                kanzashi3.SetActive(false);
                kanzashi4.SetActive(false);
                kanzashi5.SetActive(false);
                break;
            case 1:
                kanzashi1.SetActive(true);
                kanzashi2.SetActive(false);
                kanzashi3.SetActive(false);
                kanzashi4.SetActive(false);
                kanzashi5.SetActive(false);
                break;
            case 2:
                kanzashi1.SetActive(true);
                kanzashi2.SetActive(true);
                kanzashi3.SetActive(false);
                kanzashi4.SetActive(false);
                kanzashi5.SetActive(false);
                break;
            case 3:
                kanzashi1.SetActive(true);
                kanzashi2.SetActive(true);
                kanzashi3.SetActive(true);
                kanzashi4.SetActive(false);
                kanzashi5.SetActive(false);
                break;
            case 4:
                kanzashi1.SetActive(true);
                kanzashi2.SetActive(true);
                kanzashi3.SetActive(true);
                kanzashi4.SetActive(true);
                kanzashi5.SetActive(false);
                break;
            case 5:
                kanzashi1.SetActive(true);
                kanzashi2.SetActive(true);
                kanzashi3.SetActive(true);
                kanzashi4.SetActive(true);
                kanzashi5.SetActive(true);
                break;
        }
    }
}
