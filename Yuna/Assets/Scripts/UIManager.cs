using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject nearbyText; 
    public GameObject pickupText;

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

    public void ShowPickupText(string objectName)
    {
        pickupText.SetActive(true);
        pickupText.GetComponent<TextMeshProUGUI>().text = "+ 1 " + objectName;
        Invoke("HidePickupText", 2f);
    }

    private void HidePickupText()
    {
        pickupText.SetActive(false);
    }
}
