using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject nearbyText;
    public InputActionReference interactAction;
    public GameObject nearbyTextObject;

    public GameObject nearbyTextKey;
    public GameObject pickupText;
    public TextMeshProUGUI ammoText; // Adiciona um campo para exibir a munição

    [SerializeField] MenuController menuController;
    [SerializeField] GameObject helper;
    [SerializeField] TextMeshProUGUI helperTitle;
    [SerializeField] Image helperImage;
    [SerializeField] TextMeshProUGUI helperText;
    [SerializeField] GameObject weaponPanel;
    [SerializeField] GameObject DiaryEntry;

    private void Awake()
    {
        instance = this;
        nearbyText.SetActive(false);
        pickupText.SetActive(false);
    }

    public void ShowNearbyText(bool show, InteractableObject interactableObject)
    {
        nearbyText.SetActive(show);

        if (show)
        {
            var binding = interactAction.action.bindings[0];
            string key = InputControlPath.ToHumanReadableString(
                binding.effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice
            );

            nearbyTextObject.GetComponent<TextMeshProUGUI>().text = interactableObject.objectName;
            nearbyTextKey.GetComponent<TextMeshProUGUI>().text = $"{key}";
        }

    }

    private void HidePickupText()
    {
        //pickupText.SetActive(false);
    }

    public void UpdateAmmoUI(string weaponName, int amount)
    {
        if (ammoText != null)
            ammoText.text = amount.ToString(); // Atualiza o texto da munição
    }

    public void ShowHelper(string title, string text, Sprite image)
    {
        helper.SetActive(true);
        helperTitle.text = title;
        helperText.text = text;
        helperImage.sprite = image;

        weaponPanel.SetActive(true);

        menuController.PauseGame();
    }

    public void ShowDiaryEntry()
    {
        DiaryEntry.SetActive(true);

        StartCoroutine(WaitSecondsAndHide(5, DiaryEntry));
    }

    IEnumerator WaitSecondsAndHide(int seconds, GameObject gameObject)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
    }
}
