using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject nearbyText;
    public InputActionReference interactAction;
    public GameObject nearbyTextKey;
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

        if (show)
        {
            var binding = interactAction.action.bindings[0];
            string key = InputControlPath.ToHumanReadableString(
                binding.effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice
            );

            nearbyTextKey.GetComponent<TextMeshProUGUI>().text = $"{key}";
        }

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
