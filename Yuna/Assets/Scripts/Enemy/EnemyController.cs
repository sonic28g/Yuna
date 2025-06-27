using System;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private static Action _resetAllEnemiesAction;
    private static Action _saveAllEnemiesAction;

    public static void ResetAllEnemies() => _resetAllEnemiesAction?.Invoke();
    public static void SaveAllEnemies() => _saveAllEnemiesAction?.Invoke();

    private EnemyData _enemyData;
    private string _enemyDir, _enemyDataFilePath;
    private static readonly string ENEMY_DATA_FILE = "enemyData.json";

    private EnemyState _currentState;

    // Shared components
    public NavMeshAgent NavAgent { get; private set; }
    public EnemyPatrolPoints EnemyPatrolPoints { get; private set; }
    public EnemyHealth EnemyHealth { get; private set; }
    public PlayerDetection PlayerDetection { get; private set; }
    public SoundDetection SoundDetection { get; private set; }
    public Animator Animator { get; private set; }
    public AudioSource AudioSource { get; private set; }
    public Outline Outline { get; private set; }
    public Collider[] Colliders { get; private set; }

    public GameObject SearchImage;
    public GameObject ChaseImage;
    // ...

    // States
    [field: Header("States")]
    [field: SerializeField] public EnemyPatrolState PatrolState { get; private set; }
    [field: SerializeField] public EnemyChaseState ChaseState { get; private set; }
    [field: SerializeField] public EnemySearchState SearchState { get; private set; }
    [field: SerializeField] public EnemyDeadState DeadState { get; private set; }
    [field: SerializeField] public EnemyFoundState FoundState { get; private set; }
    // ...

    private static readonly string SPEED_ANIMATOR_PARAMETER = "Speed";
    private static readonly string MOTION_SPEED_ANIMATOR_PARAMETER = "MotionSpeed";

    [Header("Sound Settings")]
    [SerializeField] private AudioClip[] _footstepClips;


    private void Awake()
    {
        // Search for components
        NavAgent = GetComponentInChildren<NavMeshAgent>();
        EnemyPatrolPoints = GetComponentInChildren<EnemyPatrolPoints>();
        EnemyHealth = GetComponentInChildren<EnemyHealth>();
        PlayerDetection = GetComponentInChildren<PlayerDetection>();
        SoundDetection = GetComponentInChildren<SoundDetection>();
        Animator = GetComponentInChildren<Animator>();
        AudioSource = GetComponentInChildren<AudioSource>();
        Outline = GetComponentInChildren<Outline>();
        Colliders = GetComponentsInChildren<Collider>();

        // Check for missing components or "invalid states" + initialization
        if (NavAgent == null) throw new Exception($"NavMeshAgent is missing in {name}");
        else if (!NavAgent.isOnNavMesh) throw new Exception($"NavMeshAgent is not on the NavMesh in {name}");

        if (EnemyPatrolPoints == null) throw new Exception($"EnemyPatrolPoints is missing in {name}");
        EnemyPatrolPoints.Init();

        if (EnemyHealth == null) throw new Exception($"EnemyHealth is missing in {name}");
        else EnemyHealth.OnDeath += OnDeath;

        if (PlayerDetection == null) throw new Exception($"PlayerDetection is missing in {name}");
        if (SoundDetection == null) throw new Exception($"SoundDetection is missing in {name}");

        // Initialize states
        InitializeStates();

        // Setup save/load + Initial load
        _enemyDir = Path.Combine(Application.persistentDataPath, "Enemies", name);
        _enemyDataFilePath = Path.Combine(_enemyDir, ENEMY_DATA_FILE);
        ResetEnemy();

        _resetAllEnemiesAction += ResetEnemy;
        _saveAllEnemiesAction += SaveEnemy;
    }


    private void Start() => TransitionToState(PatrolState);
    private void OnDeath() => TransitionToState(DeadState);

    private void OnDestroy()
    {
        _resetAllEnemiesAction -= ResetEnemy;
        _saveAllEnemiesAction -= SaveEnemy;
        EnemyHealth.OnDeath -= OnDeath;
    }


    public void TransitionToState(EnemyState state)
    {
        if (_currentState != null) _currentState.ExitState(this);
        _currentState = state;
        if (_currentState != null) _currentState.EnterState(this);
    }


    private void InitializeStates()
    {
        PatrolState = InitializeState(PatrolState);
        DeadState = InitializeState(DeadState);
        ChaseState = InitializeState(ChaseState);
        FoundState = InitializeState(FoundState);
        SearchState = InitializeState(SearchState);
        // ...
    }

    private T InitializeState<T>(T state) where T : EnemyState
    {
        if (state == null) throw new Exception($"{typeof(T)} is missing in {name}");

        T newState = Instantiate(state);
        newState.name = $"{state.name} - {name}";
        newState.InitState(this);
        return newState;
    }


    private void Update()
    {
        if (_currentState != null) _currentState.UpdateState(this);
    }

    private void FixedUpdate()
    {
        UpdateAnimator();
        if (_currentState != null) _currentState.FixedUpdateState(this);
    }


    public void ResetEnemy()
    {
        LoadEnemyData();

        // Reset position and rotation
        transform.SetPositionAndRotation(_enemyData.Position, _enemyData.Rotation);
        if (NavAgent.isOnNavMesh) NavAgent.ResetPath();
        NavAgent.Warp(_enemyData.Position);

        // Reset Health and current state
        EnemyHealth.ResetHealth(_enemyDir);
        if (!EnemyHealth.IsDead) TransitionToState(PatrolState);
        else TransitionToState(DeadState);
    }

    private void LoadEnemyData()
    {
        // Already loaded
        if (_enemyData != null) return;

        try
        {
            // Read the JSON file and deserialize it + Set the data variable
            string json = File.ReadAllText(_enemyDataFilePath);
            _enemyData = JsonUtility.FromJson<EnemyData>(json) ?? throw new Exception($"Failed to parse enemy data from {_enemyDataFilePath}");
        }
        catch (Exception e)
        {
            Debug.Log($"Failed to load enemy data for {name}: {e.Message}");

            // Initialize with default values if load fails
            _enemyData = new EnemyData
            {
                Position = transform.position,
                Rotation = transform.rotation
            };
        }
    }


    public void SaveEnemy()
    {
        EnemyHealth.SaveHealth(_enemyDir);
        SaveEnemyData();
    }

    private void SaveEnemyData()
    {
        // Only if is dead
        if (!EnemyHealth.IsDead) return;

        // Save data variable
        _enemyData ??= new EnemyData();
        _enemyData.Position = transform.position;
        _enemyData.Rotation = transform.rotation;

        try
        {
            // Convert the data to JSON
            string json = JsonUtility.ToJson(_enemyData);

            // Create the directory if it doesn't exist
            if (!Directory.Exists(_enemyDir)) Directory.CreateDirectory(_enemyDir);

            // Save the JSON to a file
            File.WriteAllText(_enemyDataFilePath, json);
        }
        catch (Exception e)
        {
            Debug.Log($"Failed to save enemy data for {name}: {e.Message}");
        }
    }


    private void UpdateAnimator()
    {
        if (Animator == null) return;

        Animator.SetFloat(SPEED_ANIMATOR_PARAMETER, NavAgent.velocity.magnitude);
        Animator.SetFloat(MOTION_SPEED_ANIMATOR_PARAMETER, NavAgent.velocity.magnitude / NavAgent.speed);
    }


    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (_footstepClips.Length == 0) return;
        if (animationEvent.animatorClipInfo.weight >= 0.5f) return;


        int index = UnityEngine.Random.Range(0, _footstepClips.Length);
        AudioSource.clip = _footstepClips[index];
        AudioSource.Play();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
    private void OnLand(AnimationEvent _) {}


    [Serializable]
    private class EnemyData
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }
}
