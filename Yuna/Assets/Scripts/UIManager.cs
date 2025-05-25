using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject nearbyText; 
    public GameObject pickupText;
    public TextMeshProUGUI ammoText; // Adiciona um campo para exibir a munição

    private void Awake()
    {
        instance = this;
        nearbyText.SetActive(false);
        pickupText.SetActive(false);
    }

    public void ShowNearbyText(bool show)
    {
        nearbyText.SetActive(show);
    }

    public void ShowInteractionText(string text)
    {
        pickupText.SetActive(true);
        pickupText.GetComponent<TextMeshProUGUI>().text = text;
        Invoke("HidePickupText", 3f);
    }

    private void HidePickupText()
    {
        pickupText.SetActive(false);
    }

    public void UpdateAmmoUI(string weaponName, int amount)
    {
        if (ammoText != null)
            ammoText.text = amount.ToString(); // Atualiza o texto da munição
    }
}
