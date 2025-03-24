using System;
using UnityEngine;
using System.Collections;

public class PlayerDetection : MonoBehaviour
{
    [Header("Gizmos")]
    [SerializeField] private bool _showDistanceGizmos = true;
    [SerializeField] private bool _showDetectionGizmos = true;
    [SerializeField] private bool _showFOVGizmos = true;

    [Header("Detection Settings")]
    [SerializeField] private LayerMask _detectionLayer;
    [SerializeField] private string _playerTag = "Player";
    private Transform _playerTransform;

    [SerializeField] private Transform _eyesTransform; // Optional
    private Transform VisionOrigin => _eyesTransform ? _eyesTransform : transform;

    [Header("Detection Parameters")]
    [SerializeField] private DetectionParameters _normalParameters;
    [SerializeField] private DetectionParameters _chaseParameters;
    [SerializeField] private DetectionParameters _searchParameters;

    private DetectionMode _detectionMode = DetectionMode.Normal;
    private DetectionParameters CurrentParameters => _detectionMode switch
    {
        DetectionMode.Chase => _chaseParameters,
        DetectionMode.Search => _searchParameters,
        _ => _normalParameters,
    };

    public event Action<bool, bool, Vector3?> OnDetectionChanged; // isDetected, isTooClose, HitPoint
    public bool WasDetected { get; private set; } = false;
    public bool WasTooClose { get; private set; } = false;
    public Vector3? HitPoint { get; private set; } = null;


    private void Awake()
    {
        GameObject player = GameObject.FindWithTag(_playerTag);
        if (player) _playerTransform = player.transform;
    }

    private void OnEnable()
    {
        // Start checking detection
        StartCoroutine(nameof(CheckDetection));
    }

    private void OnDisable()
    {
        // Stop checking detection
        StopCoroutine(nameof(CheckDetection));
    }


    public void SetDetectionMode(DetectionMode mode) => _detectionMode = mode;

    private IEnumerator CheckDetection()
    {
        while (true)
        {
            DetectionParameters parameters = CurrentParameters; 
            CheckPlayerDetection(parameters);
            yield return new WaitForSeconds(parameters.DetectionCooldown);
        }
    }


    private void CheckPlayerDetection(DetectionParameters parameters)
    {
        if (_playerTransform == null) return;

        bool isDetected = CanSeePlayer(parameters, out Vector3? hitPoint);
        bool isTooClose = PlayerTooClose(parameters, hitPoint);
        
        UpdateDetectionState(isDetected, isTooClose, hitPoint);
    }

    private bool CanSeePlayer(DetectionParameters parameters, out Vector3? hitPoint)
    {
        Vector3 playerEyesPosition = _playerTransform.position;
        playerEyesPosition.y = VisionOrigin.position.y;

        Vector3 directionToPlayer = (playerEyesPosition - VisionOrigin.position).normalized;
        hitPoint = null;

        // Check field of view
        float angleToPlayer = Vector3.Angle(VisionOrigin.forward, directionToPlayer);
        if (angleToPlayer > parameters.FieldOfView / 2) return false;

        // Check line of sight w/raycast
        bool collided = Physics.Raycast(
            VisionOrigin.position, directionToPlayer,
            out RaycastHit hit,
            parameters.MaxVisionDistance, _detectionLayer
        );
        if (!collided || !hit.collider.CompareTag(_playerTag)) return false;

        hitPoint = hit.point;
        return true;
    }

    private bool PlayerTooClose(DetectionParameters parameters, Vector3? hitPoint)
    {
        if (!hitPoint.HasValue) return false;

        // Detected, check distance
        float distanceToPlayer = Vector3.Distance(VisionOrigin.position, hitPoint.Value);
        return distanceToPlayer <= parameters.MaxCloseDistance;
    }


    private void UpdateDetectionState(bool isDetected, bool isTooClose, Vector3? hitPoint)
    {
        if (isDetected == WasDetected && isTooClose == WasTooClose && hitPoint == HitPoint) return;

        // Changed detected or too close or hit point
        WasDetected = isDetected;
        WasTooClose = isTooClose;
        HitPoint = hitPoint;

        OnDetectionChanged?.Invoke(isDetected, isTooClose, hitPoint);
    }


    private void OnDrawGizmosSelected()
    {
        DetectionParameters parameters = CurrentParameters;

        if (_showFOVGizmos) DrawFOVGizmos(parameters);
        if (_showDistanceGizmos) DrawDistanceGizmos(parameters);
        if (_showDetectionGizmos) DrawDetectionGizmos(parameters);
    }

    private void DrawFOVGizmos(DetectionParameters parameters)
    {
        Gizmos.color = Color.cyan;

        float halfFOV = parameters.FieldOfView / 2;
        Vector3 leftRay = Quaternion.Euler(0, -halfFOV, 0) * VisionOrigin.forward;
        Vector3 rightRay = Quaternion.Euler(0, halfFOV, 0) * VisionOrigin.forward;

        Gizmos.DrawRay(VisionOrigin.position, leftRay * parameters.MaxVisionDistance);
        Gizmos.DrawRay(VisionOrigin.position, rightRay * parameters.MaxVisionDistance);
    }

    private void DrawDistanceGizmos(DetectionParameters parameters)
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(VisionOrigin.position, parameters.MaxVisionDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(VisionOrigin.position, parameters.MaxCloseDistance);
    }

    private void DrawDetectionGizmos(DetectionParameters parameters)
    {
        Gizmos.color = !WasDetected ? Color.green : !WasTooClose ? Color.yellow : Color.red;
        Gizmos.DrawWireSphere(VisionOrigin.position, 0.5f);
    }


    [Serializable]
    public class DetectionParameters
    {
        [field: SerializeField] public float MaxVisionDistance { get; private set; } = 10f;
        [field: SerializeField] public float MaxCloseDistance { get; private set; } = 5f;
        [field: SerializeField] public float FieldOfView { get; private set; } = 90f;
        [field: SerializeField] public float DetectionCooldown { get; private set; } = 1f;
    }

    public enum DetectionMode
    {
        Normal,
        Chase,
        Search
    }
}
