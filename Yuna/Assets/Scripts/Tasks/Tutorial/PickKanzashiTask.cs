using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Tutorial/PickKanzashi")]
public class PickKanzashiTask : TaskData
{
    private Vector3 lastPosition;
    [SerializeField] string helperTitle;
    [SerializeField] string helperText;
    [SerializeField] Sprite helperImage;

    public override void StartTask()
    {
        completed = false;
    }

    public override bool CheckIfCompleted()
    {
        if (InventoryManager.instance.GetAmmo("Kanzashi") >= 1)
        {
            UIManager.instance.ShowHelper(helperTitle, helperText, helperImage);
            completed = true;
        }
        return completed;
    }
}
