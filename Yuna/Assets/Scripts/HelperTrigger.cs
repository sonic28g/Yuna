using UnityEngine;

public class HelperTrigger : MonoBehaviour
{
    [SerializeField] string helperTitle;
    [SerializeField] string helperText;
    [SerializeField] Sprite helperImage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.instance.ShowHelper(helperTitle, helperText, helperImage);
            Destroy(gameObject);
        }
    }
}
