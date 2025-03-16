using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private EnemyState _currentState;

    // Shared components
    public NavMeshAgent NavAgent { get; private set; }
    public EnemyPatrolPoints EnemyPatrolPoints { get; private set; }
    public EnemyHealth EnemyHealth { get; private set; }
    public PlayerDetection PlayerDetection { get; private set; }
    // ...

    // States
    [field: SerializeField] public EnemyPatrolState PatrolState { get; private set; }
    [field: SerializeField] public EnemyDeadState DeadState { get; private set; }
    // ...


    private void Awake()
    {
        // Search for components
        NavAgent = GetComponentInChildren<NavMeshAgent>();
        EnemyPatrolPoints = GetComponentInChildren<EnemyPatrolPoints>();
        EnemyHealth = GetComponentInChildren<EnemyHealth>();
        PlayerDetection = GetComponentInChildren<PlayerDetection>();

        // Check for missing components or "invalid states" + initialization
        if (NavAgent == null) throw new System.Exception($"NavMeshAgent is missing in {name}");
        else if (!NavAgent.isOnNavMesh) throw new System.Exception($"NavMeshAgent is not on the NavMesh in {name}");

        if (EnemyPatrolPoints == null) throw new System.Exception($"EnemyPatrolPoints is missing in {name}");
        EnemyPatrolPoints.Init();

        if (EnemyHealth == null) throw new System.Exception($"EnemyHealth is missing in {name}");
        else EnemyHealth.OnDeath += OnDeath;

        if (PlayerDetection == null) throw new System.Exception($"PlayerDetection is missing in {name}");

        // Initialize states
        InitializeStates();
    }


    private void Start() => TransitionToState(PatrolState);
    private void OnDeath() => TransitionToState(DeadState);
    private void OnDestroy() => EnemyHealth.OnDeath -= OnDeath;


    public void TransitionToState(EnemyState state)
    {
        if (_currentState != null) _currentState.ExitState(this);
        _currentState = state;
        _currentState.EnterState(this);
    }


    private void InitializeStates()
    {
        PatrolState = InitializeState(PatrolState);
        DeadState = InitializeState(DeadState);
        // ...
    }

    private T InitializeState<T>(T state) where T : EnemyState
    {
        if (state == null) throw new System.Exception($"{typeof(T)} is missing in {name}");

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
        if (_currentState != null) _currentState.FixedUpdateState(this);
    }
}
