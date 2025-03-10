using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private EnemyState _currentState;

    // Shared components
    public NavMeshAgent NavAgent { get; private set; }
    public EnemyPatrolPoints EnemyPatrolPoints { get; private set; }
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

        // Check for missing components or "invalid states" + initialization
        if (NavAgent == null) throw new System.Exception($"NavMeshAgent is missing in {name}");
        else if (!NavAgent.isOnNavMesh) throw new System.Exception($"NavMeshAgent is not on the NavMesh in {name}");

        if (EnemyPatrolPoints == null) throw new System.Exception($"EnemyPatrolPoints is missing in {name}");
        EnemyPatrolPoints.Init();

        // Initialize states
        InitializeStates();
    }

    private void Start()
    {
        TransitionToState(PatrolState);
    }


    public void TransitionToState(EnemyState state)
    {
        if (_currentState != null) _currentState.ExitState(this);
        _currentState = state;
        _currentState.EnterState(this);
    }


    private void InitializeStates()
    {
        if (PatrolState == null) throw new System.Exception($"PatrolState is missing in {name}");
        PatrolState = Instantiate(PatrolState);
        PatrolState.name = $"PatrolState {name}";
        PatrolState.InitState(this);

        if (DeadState == null) throw new System.Exception($"DeadState is missing in {name}");
        DeadState = Instantiate(DeadState);
        DeadState.name = $"DeadState {name}";
        DeadState.InitState(this);

        // ...
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
