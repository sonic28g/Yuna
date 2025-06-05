using UnityEngine;

[CreateAssetMenu(menuName = "Yuna/Enemy/EnemyPatrolState")]
public class EnemyPatrolState : EnemyState
{
    private bool _hasPoints = false;
    private int _currentPointIndex = -1;
    private Vector3 _currentPoint = Vector3.zero;

    [SerializeField, Tooltip("Speed of the enemy during the patrol phase")]
    private float _patrolSpeed = 2f;


    [SerializeField, Tooltip("Maximum time the enemy waits at a patrol point before moving to the next one")]
    private float _maxWaitTimeAtPoint = 3f;

    [SerializeField, Tooltip("Minimum time the enemy waits at a patrol point before moving to the next one")]
    private float _minWaitTimeAtPoint = 0.5f;

    public bool IsWaiting { get; private set; }
    private float _waitTimeAtPoint = 0f;
    private float _waitStartTime = 0f;


    public override void InitState(EnemyController enemy)
    {
        _hasPoints = enemy.EnemyPatrolPoints.PatrolPoints.Length > 1;
    }

    public override void EnterState(EnemyController enemy)
    {
        // Set the patrol speed and reset the waiting state
        enemy.NavAgent.speed = _patrolSpeed;
        IsWaiting = false;

        // Move to the closest point
        ClosestPoint(enemy);
        MoveToCurrentPoint(enemy);
    }

    public override void ExitState(EnemyController enemy)
    {
        // Reset the waiting state
        IsWaiting = false;
    }


    public override void FixedUpdateState(EnemyController enemy)
    {
        // Transition to other states
        if (enemy.PlayerDetection.WasDetected)
        {
            enemy.TransitionToState(enemy.ChaseState);
            return;
        }
        else if (enemy.SoundDetection.WasSoundDetected)
        {
            enemy.TransitionToState(enemy.SearchState);
            return;
        }
        // ...

        // No patrol points available or the enemy is moving to a point
        if (!_hasPoints || enemy.NavAgent.hasPath || enemy.NavAgent.pathPending) return;

        // Start waiting if the enemy is not already waiting
        if (!IsWaiting) NewWaitTime();

        // Check if the enemy is still waiting
        float waitTime = Time.time - _waitStartTime;
        if (_waitTimeAtPoint > 0f && waitTime < _waitTimeAtPoint) return;

        // Move to the next point (and reset the waiting state)
        IsWaiting = false;
        NextPoint(enemy);
        MoveToCurrentPoint(enemy);
    }


    private void NextPoint(EnemyController enemy)
    {
        Vector3[] patrolPoints = enemy.EnemyPatrolPoints.PatrolPoints;
        int patrolPointsLength = patrolPoints.Length;

        _currentPointIndex = (_currentPointIndex + 1) % patrolPointsLength;
        _currentPoint = patrolPoints[_currentPointIndex];
    }

    private void ClosestPoint(EnemyController enemy)
    {
        Vector3[] patrolPoints = enemy.EnemyPatrolPoints.PatrolPoints;
        int patrolPointsLength = patrolPoints.Length;

        Vector3 position = enemy.transform.position;
        float closestDistance = Mathf.Infinity;
        int closestIndex = 0;

        for (int i = 0; i < patrolPointsLength; i++)
        {
            float distance = Vector3.Distance(position, patrolPoints[i]);
            if (distance >= closestDistance) continue;

            closestDistance = distance;
            closestIndex = i;
        }

        _currentPointIndex = closestIndex;
        _currentPoint = patrolPoints[_currentPointIndex];
    }


    private void NewWaitTime()
    {
        _waitStartTime = Time.time;
        _waitTimeAtPoint = Random.Range(_minWaitTimeAtPoint, _maxWaitTimeAtPoint);
        IsWaiting = true;
    }

    private void MoveToCurrentPoint(EnemyController enemy) => enemy.NavAgent.SetDestination(_currentPoint);
}
