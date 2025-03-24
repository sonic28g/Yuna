using System;
using UnityEngine;
using System.Collections;

public class PlayerDetection : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private bool _showDistanceGizmos = true;
    [SerializeField] private float _maxVisionDistance = 10f;
    [SerializeField] private float _maxCloseDistance = 5f;

    [SerializeField] private bool _showDetectionGizmos = true;
    [SerializeField] private LayerMask _detectionLayer;
    [SerializeField] private string _playerTag = "Player";
    private Transform _playerTransform;

    public event Action<bool, bool, Vector3?> OnDetectionChanged; // isDetected, isTooClose, HitPoint
    public bool WasDetected { get; private set; } = false;
    public bool WasTooClose { get; private set; } = false;
    public Vector3? HitPoint { get; private set; } = null;

    [Header("Vision Settings")]
    [SerializeField] private bool _showFOVGizmos = true;
    [SerializeField] private float _fieldOfView = 90f;
    [SerializeField] private float _detectionCooldown = 1f;
    
    [SerializeField] private Transform _eyesTransform; // Optional
    private Transform VisionOrigin => _eyesTransform ? _eyesTransform : transform;


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

    private IEnumerator CheckDetection()
    {
        while (true)
        {
            CheckPlayerDetection();
            yield return new WaitForSeconds(_detectionCooldown);
        }
    }


    private void CheckPlayerDetection()
    {
        if (_playerTransform == null) return;

        bool isDetected = CanSeePlayer(out Vector3? hitPoint);
        bool isTooClose = PlayerTooClose(hitPoint);
        
        UpdateDetectionState(isDetected, isTooClose, hitPoint);
    }

    private bool CanSeePlayer(out Vector3? hitPoint)
    {
        Vector3 playerEyesPosition = _playerTransform.position;
        playerEyesPosition.y = VisionOrigin.position.y;

        Vector3 directionToPlayer = (playerEyesPosition - VisionOrigin.position).normalized;
        hitPoint = null;

        // Check field of view
        float angleToPlayer = Vector3.Angle(VisionOrigin.forward, directionToPlayer);
        if (angleToPlayer > _fieldOfView / 2) return false;

        // Check line of sight w/raycast
        bool collided = Physics.Raycast(
            VisionOrigin.position, directionToPlayer,
            out RaycastHit hit,
            _maxVisionDistance, _detectionLayer
        );
        if (!collided || !hit.collider.CompareTag(_playerTag)) return false;

        hitPoint = hit.point;
        return true;
    }

    private bool PlayerTooClose(Vector3? hitPoint)
    {
        if (!hitPoint.HasValue) return false;

        // Detected, check distance
        float distanceToPlayer = Vector3.Distance(VisionOrigin.position, hitPoint.Value);
        return distanceToPlayer <= _maxCloseDistance;
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
        if (_showFOVGizmos) DrawFOVGizmos();
        if (_showDistanceGizmos) DrawDistanceGizmos();
        if (_showDetectionGizmos) DrawDetectionGizmos();
    }

    private void DrawFOVGizmos()
    {
        Gizmos.color = Color.cyan;

        float halfFOV = _fieldOfView / 2;
        Vector3 leftRay = Quaternion.Euler(0, -halfFOV, 0) * VisionOrigin.forward;
        Vector3 rightRay = Quaternion.Euler(0, halfFOV, 0) * VisionOrigin.forward;

        Gizmos.DrawRay(VisionOrigin.position, leftRay * _maxVisionDistance);
        Gizmos.DrawRay(VisionOrigin.position, rightRay * _maxVisionDistance);
    }

    private void DrawDistanceGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(VisionOrigin.position, _maxVisionDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(VisionOrigin.position, _maxCloseDistance);
    }

    private void DrawDetectionGizmos()
    {
        Gizmos.color = !WasDetected ? Color.green : !WasTooClose ? Color.yellow : Color.red;
        Gizmos.DrawWireSphere(VisionOrigin.position, 0.5f);
    }
}
