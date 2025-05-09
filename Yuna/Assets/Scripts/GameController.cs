using UnityEngine;

public class GameController : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = false; // Esconde o cursor
        Cursor.lockState = CursorLockMode.Locked; // Tranca o cursor ao centro do ecrã
    }
}
