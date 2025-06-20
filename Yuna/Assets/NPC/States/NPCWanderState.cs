using UnityEngine;

[CreateAssetMenu(menuName = "Yuna/NPC/NPCWanderState")]
public class NPCWanderState : NPCState
{
    private bool _hasPoints = false;
    private int _currentPointIndex = -1;
    private Vector3? _currentPoint;

    [SerializeField, Tooltip("Maximum time the npc waits at a interest point before moving to the next one")]
    private float _maxWaitTimeAtPoint = 10f;

    [SerializeField, Tooltip("Minimum time the npc waits at a interest point before moving to the next one")]
    private float _minWaitTimeAtPoint = 3.5f;

    public bool IsWaiting { get; private set; }
    private float _waitTimeAtPoint = 0f;
    private float _waitStartTime = 0f;


    public override void InitState(NPCController npc) => _hasPoints = npc.NPCInterestPoints.InterestPoints.Length > 1;

    public override void EnterState(NPCController npc) => NewWaitTime();
    public override void ExitState(NPCController npc) => IsWaiting = false;


    public override void FixedUpdateState(NPCController npc)
    {
        if (!IsWaiting)
        {
            // No interest points available or the npc is moving to a point
            if (!_hasPoints || npc.NavAgent.pathPending) return;
            if (npc.NavAgent.remainingDistance > npc.NavAgent.stoppingDistance) return;

            // Start waiting
            NewWaitTime();
        }
        else
        {
            // Check if the npc is still waiting
            float waitTime = Time.time - _waitStartTime;
            if (_waitTimeAtPoint > 0f && waitTime < _waitTimeAtPoint) return;

            // Move to the next random point (and reset the waiting state)
            IsWaiting = false;
            NextPoint(npc);
            MoveToCurrentPoint(npc);
        }
    }


    private void NextPoint(NPCController npc)
    {
        Vector3[] interestPoints = npc.NPCInterestPoints.InterestPoints;

        // If there are no interest points, do nothing
        int interestPointsLength = interestPoints.Length;
        if (interestPointsLength == 0) return;

        // Select a new random point (different from the current one)
        int newPointIndex;
        do newPointIndex = Random.Range(0, interestPointsLength);
        while (interestPointsLength > 1 && newPointIndex == _currentPointIndex);

        // Set the new point and index
        _currentPointIndex = newPointIndex;
        _currentPoint = interestPoints[_currentPointIndex];
    }

    private void NewWaitTime()
    {
        _waitStartTime = Time.time;
        _waitTimeAtPoint = Random.Range(_minWaitTimeAtPoint, _maxWaitTimeAtPoint);
        IsWaiting = true;
    }

    private void MoveToCurrentPoint(NPCController npc)
    {
        if (_currentPoint.HasValue) npc.NavAgent.SetDestination(_currentPoint.Value);
    }
}

