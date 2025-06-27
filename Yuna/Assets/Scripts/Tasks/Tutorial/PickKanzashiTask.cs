using System.Collections;
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
        UIManager.instance.ShowHelper(helperTitle, helperText, helperImage);
    }

    public override bool CheckIfCompleted()
    {
        if (InventoryManager.instance.GetAmmo("Kanzashi") >= 1)
        {
            completed = true;
        }
        return completed;
    }

}
