using UnityEngine;
using System;

[RequireComponent(typeof(BoxCollider))]
public class PlayerAreaTrigger : MonoBehaviour
{
    [SerializeField] private bool _showGizmos = true;

    private static readonly string PLAYER_TAG = "Player";
    public static event Action<PlayerAreaTrigger, bool> OnPlayerAreaChanged;
    [field: SerializeField] public PlayerArea AreaType { get; private set; } = PlayerArea.Normal;


    private void Awake()
    {
        // Ensure the colliders are set to trigger
        BoxCollider[] colliders = GetComponents<BoxCollider>();
        if (colliders.Length > 1) Debug.LogError($"The use of multiple colliders in {name} is not recommended.\nThis may cause unexpected behavior in PlayerAreaTrigger");
        
        foreach (BoxCollider collider in colliders) collider.isTrigger = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(PLAYER_TAG)) return;
        OnPlayerAreaChanged?.Invoke(this, true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(PLAYER_TAG)) return;
        OnPlayerAreaChanged?.Invoke(this, false);
    }


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
        foreach (BoxCollider collider in colliders)
        {
            Gizmos.matrix = collider.transform.localToWorldMatrix;
            Gizmos.DrawCube(collider.bounds.center, collider.bounds.size);
        }
        Gizmos.matrix = Matrix4x4.identity;
    }
}


public enum PlayerArea
{
    Safe,
    Normal,
    Suspicious
}
