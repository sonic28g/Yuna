using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitcher : MonoBehaviour
{
    public GameObject[] weapons;
    public WeaponData[] weaponDatas;
    private int currentWeaponIndex = 0;

    [SerializeField] private StarterAssetsInputs _inputs;

    [Header("UI")]
    [SerializeField] private Sprite imageWeapon1;
    [SerializeField] private Sprite imageWeapon2;
    [SerializeField] private Image weaponPanelImage1;
    [SerializeField] private Image weaponPanelImage2;
    [SerializeField] private GameObject ammoValue;
    [SerializeField] private GameObject tessen;

    private WeaponData currentWeaponData;
    public static WeaponSwitcher instance;
    public WeaponData CurrentWeapon => currentWeaponData;

    public bool canSwitchWeapons = false; // Ativado quando o jogador apanha o Tessen

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        EquipWeapon(0); // Kanzashi por defeito
        weaponPanelImage2.gameObject.SetActive(canSwitchWeapons);
    }

    void Update()
    {
        if (!canSwitchWeapons) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipWeapon(0);
            tessen.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipWeapon(1);
            tessen.SetActive(true);
        }
    }

    void EquipWeapon(int index)
    {
        currentWeaponIndex = index;
        currentWeaponData = weaponDatas[index];

        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(i == index);
        }

        // Atualiza UI
        if (index == 0)
        {
            weaponPanelImage1.sprite = imageWeapon1;
            weaponPanelImage2.sprite = imageWeapon2;
        }
        else
        {
            weaponPanelImage1.sprite = imageWeapon2;
            weaponPanelImage2.sprite = imageWeapon1;
        }

        // Munição só se for Kanzashi
        ammoValue.SetActive(currentWeaponData.weaponType == WeaponType.Kanzashi);
    }

    public bool IsKanzashiEquipped => currentWeaponData.weaponType == WeaponType.Kanzashi;
    public bool IsTessenEquipped => currentWeaponData.weaponType == WeaponType.Tessen;
}