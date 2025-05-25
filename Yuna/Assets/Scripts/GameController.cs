using UnityEngine;
using UnityEngine.UI;
using StarterAssets;
using TMPro;

public class GameController : MonoBehaviour
{

    void Awake()
    {
        //GameSettings settings = GameSettings.LoadFromFile();
        //SettingsManager.SetQualityLevel(settings.qualityLevel);
    }

    void Start()
    {
        Cursor.visible = false; // Esconde o cursor
        Cursor.lockState = CursorLockMode.Locked; // Tranca o cursor ao centro do ecrã
    }

    private void Update() 
    {

    }



}
