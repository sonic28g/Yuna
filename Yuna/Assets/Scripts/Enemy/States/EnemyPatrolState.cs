using UnityEngine;

[CreateAssetMenu(menuName = "Yuna/Enemy/EnemyPatrolState")]
public class EnemyPatrolState : EnemyState
{
    private bool hasPoints = false;
    private int currentPointIndex = -1;
    private Vector3 currentPoint = Vector3.zero;


    public override void InitState(EnemyController enemy)
    {
        hasPoints = enemy.EnemyPatrolPoints.PatrolPoints.Length > 1;
    }


    public override void EnterState(EnemyController enemy)
    {
        // Move to the closest point
        if (!hasPoints) return;
        ClosestPoint(enemy);
        MoveToCurrentPoint(enemy);
    }

    public override void FixedUpdateState(EnemyController enemy)
    {
        // Transition to other states
        // ...

        // Move to the next point
        if (!hasPoints || enemy.NavAgent.hasPath) return;
        NextPoint(enemy);
        MoveToCurrentPoint(enemy);
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


    private void MoveToCurrentPoint(EnemyController enemy)
    {
        enemy.NavAgent.SetDestination(currentPoint);
    }
}
