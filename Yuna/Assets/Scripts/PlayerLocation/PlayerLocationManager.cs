using UnityEngine;

public class PlayerLocationManager : MonoBehaviour
{
    public static PlayerLocationManager instance;

    public string currentZoneName;

    [SerializeField] Transform marker;
    [SerializeField] Transform castleMapLocation;

    [SerializeField] Transform marketMapLocation;
    [SerializeField] Transform eastVillageMapLocation;
    [SerializeField] Transform southVillageMapLocation;


    private void Awake()
    {
        // Garante que existe apenas uma instância
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // opcional, se quiseres manter entre cenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdatePlayerZone(string newZoneName)
    {
        switch (newZoneName)
        {
            case "Castle":
                marker.transform.position = castleMapLocation.position;
                break;
            case "Market":
                marker.transform.position = marketMapLocation.position;
                break;
            case "VillageEast":
                marker.transform.position = eastVillageMapLocation.position;
                break;
            case "VillageSouth":
                marker.transform.position = southVillageMapLocation.position;
                break;
        }
    }
}