using UnityEngine;
using System;

[RequireComponent(typeof(BoxCollider))]
public class PlayerAreaTrigger : MonoBehaviour
{
    [SerializeField] private bool _showGizmos = true;

    public static event Action<PlayerAreaTrigger, bool> OnPlayerAreaChanged;
    [field: SerializeField] public PlayerArea AreaType { get; private set; } = PlayerArea.Normal;


    private void Awake()
    {
        // Ensure the colliders are set to trigger
        BoxCollider[] colliders = GetComponents<BoxCollider>();
        foreach (BoxCollider collider in colliders) collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider _) => OnPlayerAreaChanged?.Invoke(this, true);
    private void OnTriggerExit(Collider _) => OnPlayerAreaChanged?.Invoke(this, false);


    private void OnDrawGizmosSelected()
    {
        if (!_showGizmos) return;

        // Select the color based on the area type
        Gizmos.color = AreaType switch
        {
            PlayerArea.Safe => Color.green,
            PlayerArea.Normal => Color.yellow,
            PlayerArea.Suspicious => Color.red,
            _ => Color.white
        } * new Color(1, 1, 1, 0.5f);

        // Draw the gizmos for each collider
        BoxCollider[] colliders = GetComponents<BoxCollider>();
        foreach (BoxCollider collider in colliders) Gizmos.DrawCube(collider.bounds.center, collider.bounds.size);
    }
}


public enum PlayerArea
{
    Safe,
    Normal,
    Suspicious
}
