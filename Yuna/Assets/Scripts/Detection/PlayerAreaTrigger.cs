using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class PlayerAreaTrigger : MonoBehaviour
{
    public static event Action<PlayerAreaTrigger, bool> OnPlayerAreaChanged;
    [field: SerializeField] public PlayerArea AreaType { get; private set; } = PlayerArea.Normal;


    private void OnTriggerEnter(Collider _) => OnPlayerAreaChanged?.Invoke(this, true);
    private void OnTriggerExit(Collider _) => OnPlayerAreaChanged?.Invoke(this, false);
}


public enum PlayerArea
{
    Safe,
    Normal,
    Suspicious
}
