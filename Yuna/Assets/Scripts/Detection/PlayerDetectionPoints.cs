using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDetectionPoints : MonoBehaviour
{
    [Header("Gizmos")]
    [SerializeField] private bool _showPointsGizmos = true;
    [SerializeField] private bool _showDetectedLine = true;

    [Header("Detection Properties")]
    [SerializeField] private LayerMask _detectionLayer;
    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private Transform[] _detectionPoints;

    // TODO: For testing purposes - will be removed when State is changed automatically
    [Header("State"), SerializeField]
    private PlayerState _currentState = PlayerState.Normal;

    private readonly List<PlayerAreaTrigger> _currentAreas = new();
    private PlayerArea _currentArea = PlayerArea.Normal;

    public bool IsInSafeArea() => _currentArea == PlayerArea.Safe;
    public bool IsSuspicious() => _currentArea == PlayerArea.Suspicious || _currentState == PlayerState.Suspicious;


    private void OnEnable() => PlayerAreaTrigger.OnPlayerAreaChanged += HandleAreaChanged;
    private void OnDisable() => PlayerAreaTrigger.OnPlayerAreaChanged -= HandleAreaChanged;

    private void HandleAreaChanged(PlayerAreaTrigger trigger, bool entered)
    {
        if (trigger == null) return;

        // Add or remove the trigger from the list
        if (entered && !_currentAreas.Contains(trigger))
            _currentAreas.Add(trigger);
        else if (!entered && _currentAreas.Contains(trigger))
            _currentAreas.Remove(trigger);

        UpdateCurrentArea();
    }

    private void UpdateCurrentArea()
    {
        // Exists at least one suspicious area
        if (_currentAreas.Any(a => a.AreaType == PlayerArea.Suspicious))
            _currentArea = PlayerArea.Suspicious;

        // Exists at least one normal area
        else if (_currentAreas.Any(a => a.AreaType == PlayerArea.Normal))
            _currentArea = PlayerArea.Normal;

        // Exists at least one safe area
        else if (_currentAreas.Any(a => a.AreaType == PlayerArea.Safe))
            _currentArea = PlayerArea.Safe;

        // no areas
        else _currentArea = PlayerArea.Normal;
    }


    public bool InLineOfSight(
        Vector3 origin, Vector3 originDirection,
        float maxDistance, float fieldOfView,
        out Vector3? hitPoint
    )
    {
        hitPoint = null;

        // No points to check
        if (_detectionPoints == null || _detectionPoints.Length == 0) return false;

        float maxDistanceSqr = maxDistance * maxDistance;
        float halfFOV = fieldOfView / 2;

        foreach (Transform point in _detectionPoints)
        {
            // Invalid point
            if (point == null) continue;

            Vector3 targetPos = point.position;
            Vector3 targetOriginDiff = targetPos - origin;

            Vector3 directionToTarget = targetOriginDiff.normalized;
            float distanceSqr = targetOriginDiff.sqrMagnitude;

            // Check field of view
            float angle = Vector3.Angle(originDirection, directionToTarget);
            if (angle > halfFOV) continue;

            // Check distance
            if (distanceSqr > maxDistanceSqr) continue;

            // Check line of sight
            bool hitSomething = Physics.Linecast(origin, targetPos, out RaycastHit hit, _detectionLayer);
            if (hitSomething && !hit.collider.CompareTag(_playerTag)) continue;

            if (_showDetectedLine) Debug.DrawLine(origin, targetPos, Color.magenta, 0.1f);
            hitPoint = hitSomething ? hit.point : targetPos;
            return true;
        }

        return false;
    }


    private void OnDrawGizmosSelected()
    {
        // Not showing gizmos or no points to show
        if (!_showPointsGizmos || _detectionPoints == null || _detectionPoints.Length == 0) return;

        Gizmos.color = Color.red;
        foreach (Transform point in _detectionPoints)
        {
            // Invalid point
            if (point == null) continue;

            Gizmos.DrawSphere(point.position, 0.1f);
        }
    }
}


public enum PlayerState
{
    Normal,
    Suspicious
}
