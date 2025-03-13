using System.Linq;
using UnityEngine;

public class EnemyPatrolPoints : MonoBehaviour
{
    [SerializeField] private bool _showGizmos = true;

    public Vector3[] PatrolPoints { get; private set; }
    private bool isInitialized = false;

    private Transform[] GetPatrolPointTransforms() => GetComponentsInChildren<Transform>();
    private Vector3[] GetPatrolPoints(Transform[] transforms) => transforms.Select(point => point.position).ToArray();


    public void Init()
    {
        if (isInitialized) return;

        // Get the patrol points from the transform array
        Transform[] transforms = GetPatrolPointTransforms();
        PatrolPoints = GetPatrolPoints(transforms);

        isInitialized = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (!_showGizmos) return;

        // Get the patrol points from the transform array
        Vector3[] patrolPoints = PatrolPoints;
        if (!isInitialized)
        {
            Transform[] transforms = GetPatrolPointTransforms();
            patrolPoints = GetPatrolPoints(transforms);
        }

        // Skip drawing if there are no patrol points
        int patrolPointsLength = patrolPoints.Length;
        if (patrolPointsLength == 0) return;

        Gizmos.color = Color.green;
        for (int i = 0; i < patrolPointsLength; i++)
        {
            int nextIndex = (i + 1) % patrolPointsLength;

            Vector3 point = patrolPoints[i];
            Vector3 nextPoint = patrolPoints[nextIndex];

            // Draw the patrol points and lines between them
            Gizmos.DrawSphere(point, 0.1f);
            Gizmos.DrawLine(point, nextPoint);
        }
    }
}
