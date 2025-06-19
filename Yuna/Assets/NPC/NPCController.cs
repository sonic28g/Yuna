using System;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    private static Action _resetAllNPCsAction;
    private static Action _saveAllNPCsAction;

    public static void ResetAllNPCs() => _resetAllNPCsAction?.Invoke();
    public static void SaveAllNPCs() => _saveAllNPCsAction?.Invoke();

    private NPCState _currentState;

    // Shared components
    public NavMeshAgent NavAgent { get; private set; }
    // public NPCInterestPoints NPCInterestPoints { get; private set; }
    public EnemyHealth EnemyHealth { get; private set; }
    public Animator Animator { get; private set; }
    public AudioSource AudioSource { get; private set; }
    // ...

    // States
    [field: Header("States")]
    [field: SerializeField] public NPCWanderState WanderState { get; private set; }
    [field: SerializeField] public NPCDeadState DeadState { get; private set; }
    // ...

    private static readonly string SPEED_ANIMATOR_PARAMETER = "Speed";
    private static readonly string MOTION_SPEED_ANIMATOR_PARAMETER = "MotionSpeed";
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;

    [Header("Sound Settings")]
    [SerializeField] private AudioClip[] _footstepClips;


    private void Awake()
    {
        // Search for components
        NavAgent = GetComponentInChildren<NavMeshAgent>();
        // NPCInterestPoints = GetComponentInChildren<NPCInterestPoints>();
        EnemyHealth = GetComponentInChildren<EnemyHealth>();
        Animator = GetComponentInChildren<Animator>();
        AudioSource = GetComponentInChildren<AudioSource>();

        // Store initial position and rotation
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;

        // Check for missing components or "invalid states" + initialization
        if (NavAgent == null) throw new Exception($"NavMeshAgent is missing in {name}");
        else if (!NavAgent.isOnNavMesh) throw new Exception($"NavMeshAgent is not on the NavMesh in {name}");

        // if (NPCInterestPoints == null) throw new Exception($"NPCInterestPoints is missing in {name}");
        // NPCInterestPoints.Init();

        if (EnemyHealth == null) throw new Exception($"EnemyHealth is missing in {name}");
        else EnemyHealth.OnDeath += OnDeath;

        // Initialize states
        InitializeStates();

        _resetAllNPCsAction += ResetNPC;
        _saveAllNPCsAction += SaveNPC;
    }


    private void Start() => TransitionToState(WanderState);
    private void OnDeath() => TransitionToState(DeadState);
    private void OnDestroy() => EnemyHealth.OnDeath -= OnDeath;


    public void TransitionToState(NPCState state)
    {
        if (_currentState != null) _currentState.ExitState(this);
        _currentState = state;
        if (_currentState != null) _currentState.EnterState(this);
    }


    private void InitializeStates()
    {
        WanderState = InitializeState(WanderState);
        DeadState = InitializeState(DeadState);
        // ...
    }

    private T InitializeState<T>(T state) where T : NPCState
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


    public void ResetNPC()
    {
        // Reset position and rotation
        transform.SetPositionAndRotation(_initialPosition, _initialRotation);
        if (NavAgent.isOnNavMesh) NavAgent.ResetPath();
        NavAgent.Warp(_initialPosition);

        // Reset Health and current state
        EnemyHealth.ResetHealth();
        TransitionToState(WanderState);
    }

    public void SaveNPC()
    {
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
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
    private void OnLand(AnimationEvent _) { }
}
