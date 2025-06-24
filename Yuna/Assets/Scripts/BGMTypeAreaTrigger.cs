using UnityEngine;
using System;

[RequireComponent(typeof(BoxCollider))]
public class BGMTypeAreaTrigger : MonoBehaviour
{
    [SerializeField] private bool _showGizmos = true;

    private static readonly string PLAYER_TAG = "Player";
    public static event Action<BGMTypeAreaTrigger, bool> OnBGMTypeAreaChanged;
    [field: SerializeField] public BGMType BGMType { get; private set; } = BGMType.Outside;


    private void Awake()
    {
        // Ensure the colliders are set to trigger
        BoxCollider[] colliders = GetComponents<BoxCollider>();
        if (colliders.Length > 1) Debug.LogError($"The use of multiple colliders in {name} is not recommended.\nThis may cause unexpected behavior in BGMTypeAreaTrigger");

        foreach (BoxCollider collider in colliders) collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(PLAYER_TAG)) return;
        OnBGMTypeAreaChanged?.Invoke(this, true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(PLAYER_TAG)) return;
        OnBGMTypeAreaChanged?.Invoke(this, false);
    }


    private void OnDrawGizmosSelected()
    {
        if (!_showGizmos) return;

        // Select the color based on the area type
        Gizmos.color = BGMType switch
        {
            BGMType.Outside => Color.white,
            BGMType.YunaHouse => Color.magenta,
            BGMType.Village => Color.green,
            BGMType.GuardHouse => Color.blue,
            BGMType.Werehouse => Color.yellow,
            BGMType.Market => Color.red,
            _ => Color.black
        } * new Color(1, 1, 1, 0.5f);

        // Draw the gizmos for each collider
        BoxCollider[] colliders = GetComponents<BoxCollider>();
        foreach (BoxCollider collider in colliders)
        {
            Gizmos.matrix = collider.transform.localToWorldMatrix;
            Gizmos.DrawCube(collider.center, collider.size);
        }
        Gizmos.matrix = Matrix4x4.identity;
    }
}
