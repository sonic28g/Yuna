using System;
using UnityEngine;

public class SoundDetection : MonoBehaviour
{
    private bool _enabled = true;
    [SerializeField] private bool _showDetectionGizmos = true;

    public event Action<Vector3> OnDetectionChange;
    public Vector3? LastSoundPosition { get; private set; } = null;

    public bool WasSoundDetected => LastSoundPosition != _lastCheckPosition;
    private Vector3? _lastCheckPosition = null;


    private void Awake() => _enabled = enabled;

    public void EnableDetection() => enabled = _enabled;
    
    public void DisableDetection() => enabled = false;
    public void OnDisable() => CheckedLastPosition();


    public void SoundDetectedInPosition(Vector3 soundPosition)
    {
        if (!enabled) return;

        LastSoundPosition = soundPosition;
        OnDetectionChange?.Invoke(soundPosition);
    }

    public void CheckedLastPosition() => _lastCheckPosition = LastSoundPosition;


    private void OnDrawGizmosSelected()
    {
        if (!_showDetectionGizmos) return;

        Gizmos.color = WasSoundDetected ? Color.red : Color.green;
        if (LastSoundPosition.HasValue) Gizmos.DrawWireSphere(LastSoundPosition.Value, 0.5f);
    }
}
