using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    public static BGMPlayer Instance { get; private set; }


    private void Awake()
    {
        // Singleton pattern
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public void Play()
    {
    }

    public void Stop()
    {
    }


    public void Muffle()
    {
    }

    public void Unmuffle()
    {
    }
}
