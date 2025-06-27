using UnityEngine;
using UnityEngine.UI;

public class KeybindsPanelController : MonoBehaviour
{
    [SerializeField] private GameObject page1;
    [SerializeField] private GameObject page2;

    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;

    private int currentPageIndex = 1; // 1 = page1, 2 = page2

    void Start()
    {
        ShowPage(1);
    }

    public void nextPage()
    {
        if (currentPageIndex == 1)
        {
            ShowPage(2);
        }
    }

    public void previousPage()
    {
        if (currentPageIndex == 2)
        {
            ShowPage(1);
        }
    }

    private void ShowPage(int pageIndex)
    {
        currentPageIndex = pageIndex;

        page1.SetActive(pageIndex == 1);
        page2.SetActive(pageIndex == 2);

        // Atualizar botões com base na página atual
        previousButton.interactable = (currentPageIndex != 1);
        nextButton.interactable = (currentPageIndex != 2);
    }
}
