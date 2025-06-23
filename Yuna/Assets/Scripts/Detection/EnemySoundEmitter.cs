using UnityEngine;
using UnityEngine.AI;

public class EnemySoundEmitter : MonoBehaviour
{
    // Distance for sample NavMesh points + Time to destroy the emitter
    private const float MAX_SAMPLE_DISTANCE = 1f;
    private const float DESTRUCTION_TIME = 5f;

    [Header("Emission Settings")]
    [SerializeField] private bool _showEmissionGizmos = true;
    [SerializeField, Tooltip("Layers that can hear the sound")]
    private LayerMask _enemyMask;
    [SerializeField, Tooltip("Initial sphere radius to check for potential enemies")]
    private float _checkRadius = 8f;
    [SerializeField, Tooltip("Range to notify enemies")]
    private float _notifyRange = 5f;

    [Header("Activation Settings")]
    [SerializeField, Tooltip("Time between the activation and the sound emission")]
    private float _timeToActivate = 0f;
    [SerializeField, Tooltip("Time between each activation (if repeat is enabled)")]
    private float _timeBetweenActivations = 5f;
    [SerializeField] private bool _repeat = false;

    [Header("Sound Settings")]
    [SerializeField] private AudioClip[] _clips;
    private AudioSource _audioSource;


    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_repeat) InvokeRepeating(nameof(Activation), 0f, _timeBetweenActivations);
        else
        {
            Invoke(nameof(Activation), 0f);
            Invoke(nameof(DestroyThis), _timeToActivate + DESTRUCTION_TIME);
        }
    }

    private void Activation() => Invoke(nameof(Emission), _timeToActivate);
    private void DestroyThis() => Destroy(gameObject);

    private void Emission()
    {
        EmitSound();
        NotifyEnemies();
    }

    private void EmitSound()
    {
        if (_audioSource == null || _clips.Length == 0) return;

        // Random sound from the list
        int randomIndex = Random.Range(0, _clips.Length);
        _audioSource.clip = _clips[randomIndex];
        _audioSource.Play();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "UNT0028:Use non-allocating physics APIs", Justification = "<Pending>")]
    private void NotifyEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _checkRadius, _enemyMask);
        Vector3 emitterPosition = transform.position;

        foreach (Collider enemy in colliders)
        {
            if (enemy == null) continue;

            // Check if they have SoundDetection
            bool hasSoundDetection = enemy.TryGetComponent(out SoundDetection soundDetection);
            if (!hasSoundDetection) continue;

            // Check if they are in range (with NavMesh)
            Vector3 enemyPosition = enemy.transform.position;
            float? navDistance = GetNavMeshDistance(emitterPosition, enemyPosition);
            if (!navDistance.HasValue || navDistance.Value > _notifyRange) continue;

            // Detect sound
            soundDetection.SoundDetectedInPosition(emitterPosition);
        }
    }


    private float? GetNavMeshDistance(Vector3 start, Vector3 end)
    {
        // Snap to closest point on NavMesh
        bool startSnapped = NavMesh.SamplePosition(start, out NavMeshHit startHit, MAX_SAMPLE_DISTANCE, NavMesh.AllAreas);
        bool endSnapped = NavMesh.SamplePosition(end, out NavMeshHit endHit, MAX_SAMPLE_DISTANCE, NavMesh.AllAreas);
        if (!startSnapped || !endSnapped) return null;

        // Calculate the path
        NavMeshPath path = new();
        bool foundPath = NavMesh.CalculatePath(startHit.position, endHit.position, NavMesh.AllAreas, path);

        // Path not found or incomplete
        if (!foundPath || path.status != NavMeshPathStatus.PathComplete) return null;

        // Get distance
        float pathDistance = 0f;
        for (int i = 1; i < path.corners.Length; i++)
        {
            pathDistance += Vector3.Distance(path.corners[i - 1], path.corners[i]);

            if (!_showEmissionGizmos) continue;
            Color orange50 = new(1, 0.5f, 0, 0.5f); // Orange 50%
            Debug.DrawLine(path.corners[i - 1], path.corners[i], orange50, 2f);
        }

        return pathDistance;
    }


    private void OnDrawGizmosSelected()
    {
        if (!_showEmissionGizmos) return;

        Gizmos.color = new Color(1, 0.5f, 0, 0.5f); // Orange 50%
        Gizmos.DrawSphere(transform.position, _checkRadius);
    }
}
