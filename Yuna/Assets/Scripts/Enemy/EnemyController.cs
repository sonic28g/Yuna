using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private static Action _resetAllEnemiesAction;
    private static Action _saveAllEnemiesAction;

    public static void ResetAllEnemies() => _resetAllEnemiesAction?.Invoke();
    public static void SaveAllEnemies() => _saveAllEnemiesAction?.Invoke();

    private EnemyData _enemyData;

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
        ResetEnemy();

        _resetAllEnemiesAction += ResetEnemy;
        _saveAllEnemiesAction += SaveEnemy;
    }


    private void Start() => TransitionToState(PatrolState);
    private void OnDeath() => TransitionToState(DeadState);
    private void OnDestroy() => EnemyHealth.OnDeath -= OnDeath;


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
        EnemyHealth.ResetHealth();
        if (!EnemyHealth.IsDead) TransitionToState(PatrolState);
        else TransitionToState(DeadState);
    }

    private void LoadEnemyData()
    {
        // Already loaded
        if (_enemyData != null) return;

        _enemyData = new EnemyData
        {
            Position = transform.position,
            Rotation = transform.rotation
        };
    }


    public void SaveEnemy()
    {
        EnemyHealth.SaveHealth();
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
