using UnityEngine;

[CreateAssetMenu(menuName = "Yuna/Enemy/EnemyPatrolState")]
public class EnemyPatrolState : EnemyState
{
    private bool _hasPoints = false;
    private int _currentPointIndex = -1;
    private Vector3 _currentPoint = Vector3.zero;

    [SerializeField, Tooltip("Speed of the enemy during the patrol phase")]
    private float _patrolSpeed = 2f;


    public override void InitState(EnemyController enemy)
    {
        _hasPoints = enemy.EnemyPatrolPoints.PatrolPoints.Length > 1;
    }

    public override void EnterState(EnemyController enemy)
    {
        // Set the patrol speed
        enemy.NavAgent.speed = _patrolSpeed;

        // Move to the closest point
        ClosestPoint(enemy);
        MoveToCurrentPoint(enemy);
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

        // Move to the next point if the current point is reached
        if (!_hasPoints || enemy.NavAgent.hasPath || enemy.NavAgent.pathPending) return;
        
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


    private void MoveToCurrentPoint(EnemyController enemy) => enemy.NavAgent.SetDestination(_currentPoint);
}
