using UnityEngine;

public class EnemySoundEmitter : MonoBehaviour
{
    // Time to destroy the emitter
    private const float DESTRUCTION_TIME = 5f;

    [Header("Emission Settings")]
    [SerializeField] private bool _showEmissionGizmos = true;
    [SerializeField, Tooltip("Layers that can hear the sound")]
    private LayerMask _enemyMask;
    [SerializeField, Tooltip("Initial sphere radius to check for potential enemies")]
    private float _checkRadius = 8f;

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

            // Detect sound
            soundDetection.SoundDetectedInPosition(emitterPosition);
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (!_showEmissionGizmos) return;

        Gizmos.color = new Color(1, 0.5f, 0, 0.5f); // Orange 50%
        Gizmos.DrawSphere(transform.position, _checkRadius);
    }
}
