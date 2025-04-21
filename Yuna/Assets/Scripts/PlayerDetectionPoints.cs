using UnityEngine;

public class PlayerDetectionPoints : MonoBehaviour
{
    [SerializeField] private bool _showPointsGizmos = true;

    [Header("Detection Properties")]
    [SerializeField] private LayerMask _detectionLayer;
    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private Transform[] _detectionPoints;

    // TODO: For testing purposes - will be removed when both Area and State are changed automatically
    [Header("Area + State")]
    [SerializeField] private PlayerArea _currentArea = PlayerArea.Normal;
    [SerializeField] private PlayerState _currentState = PlayerState.Normal;


    public void SetCurrentArea(PlayerArea area) => _currentArea = area;
    public void SetCurrentPlayerState(PlayerState state) => _currentState = state;

    public bool IsInSafeArea() => _currentArea == PlayerArea.Safe;
    public bool IsSuspicious() => _currentArea == PlayerArea.Suspicious || _currentState == PlayerState.Suspicious;


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

            Debug.DrawLine(origin, targetPos, Color.magenta, 0.1f);
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


    public enum PlayerArea
    {
        Safe,
        Normal,
        Suspicious
    }

    public enum PlayerState
    {
        Normal,
        Suspicious
    }
}
