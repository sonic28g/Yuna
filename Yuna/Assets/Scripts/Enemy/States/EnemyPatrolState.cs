using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Yuna/Enemy/EnemyPatrolState")]
public class EnemyPatrolState : EnemyState
{
    private bool hasPoints = false;
    private int currentPointIndex = -1;
    private Vector3 currentPoint = Vector3.zero;

    private NavMeshAgent _agent;


    public override void InitState(EnemyController enemy)
    {
        _agent = enemy.GetComponentInChildren<NavMeshAgent>();
        if (_agent == null) throw new System.Exception($"NavMeshAgent is missing in {enemy.name}");
        else if (!_agent.isOnNavMesh) throw new System.Exception($"NavMeshAgent is not on the NavMesh in {enemy.name}");

        hasPoints = enemy.EnemyPatrolPoints.PatrolPoints.Length > 1;
    }


    public override void EnterState(EnemyController enemy)
    {
        if (!hasPoints) return;

        ClosestPoint(enemy);
        MoveToCurrentPoint();
    }

    public override void FixedUpdateState(EnemyController enemy)
    {
        // Transition to other states
        // ...

        if (!hasPoints) return;
        if (_agent.hasPath || _agent.pathStatus != NavMeshPathStatus.PathComplete) return;

        NextPoint(enemy);
        MoveToCurrentPoint();
    }


    private void NextPoint(EnemyController enemy)
    {
        Vector3[] patrolPoints = enemy.EnemyPatrolPoints.PatrolPoints;
        int patrolPointsLength = patrolPoints.Length;

        currentPointIndex = (currentPointIndex + 1) % patrolPointsLength;
        currentPoint = patrolPoints[currentPointIndex];
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
            if (distance >= closestDistance) return;

            closestDistance = distance;
            closestIndex = i;
        }

        currentPointIndex = closestIndex;
        currentPoint = patrolPoints[currentPointIndex];
    }


    private void MoveToCurrentPoint()
    {
        _agent.SetDestination(currentPoint);
    }
}
