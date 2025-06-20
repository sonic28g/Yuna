using System.Linq;
using UnityEngine;

public class NPCInterestPoints : MonoBehaviour
{
    [SerializeField] private bool _showGizmos = true;

    public Vector3[] InterestPoints { get; private set; }
    private bool isInitialized = false;

    [SerializeField] private Transform[] _interestPoints;
    private Vector3[] GetInterestPoints() => _interestPoints.Where(point => point != null).Select(point => point.position).ToArray() ?? new Vector3[0];


    public void Init()
    {
        if (isInitialized) return;

        InterestPoints = GetInterestPoints();
        isInitialized = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (!_showGizmos) return;

        // Get the interest points from the transform array
        Vector3[] interestPoints = isInitialized ? InterestPoints : GetInterestPoints();

        // Draw the interest points and lines between them and the current position
        Gizmos.color = Color.green;
        foreach (Vector3 point in interestPoints)
        {
            Gizmos.DrawSphere(point, 0.1f);
            Gizmos.DrawLine(transform.position, point);
        }
    }
}
