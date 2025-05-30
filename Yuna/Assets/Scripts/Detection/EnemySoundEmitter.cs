using UnityEngine;

public class EnemySoundEmitter : MonoBehaviour
{
    [Header("Emission Settings")]
    [SerializeField] private bool _showEmissionGizmos = true;
    [SerializeField] private float _radius = 5f;
    [SerializeField] private LayerMask _enemyMask;
    [SerializeField] private int _maxEnemies = 8;
    private Collider[] _colliders;

    [Header("Activation Settings")]
    [SerializeField, Tooltip("Time between the activation and the sound emission")]
    private float _timeToActivate = 0f;
    [SerializeField, Tooltip("Time between each activation (if repeat is enabled)")]
    private float _timeBetweenActivations = 5f;
    [SerializeField] private bool _repeat = false;

    private const float DESTRUCTION_TIME = 2f;

    [Header("Sound Settings")]
    [SerializeField] private AudioClip[] _clips;
    private AudioSource _audioSource;


    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _colliders = new Collider[_maxEnemies];

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

        int randomIndex = Random.Range(0, _clips.Length);
        _audioSource.clip = _clips[randomIndex];
        _audioSource.Play();
    }

    private void NotifyEnemies()
    {
        int numEnemies = Physics.OverlapSphereNonAlloc(transform.position, _radius, _colliders, _enemyMask);
        if (numEnemies == 0) return;

        Vector3 emitterPosition = transform.position;
        for (int i = 0; i < numEnemies; i++)
        {
            bool hasSoundDetection = _colliders[i].TryGetComponent(out SoundDetection soundDetection);
            if (hasSoundDetection) soundDetection.SoundDetectedInPosition(emitterPosition);
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (!_showEmissionGizmos) return;

        Gizmos.color = new Color(1, 0.5f, 0, 0.5f); // Orange 50%
        Gizmos.DrawSphere(transform.position, _radius);
    }
}
