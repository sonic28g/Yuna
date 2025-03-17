using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Yuna/Enemy/EnemyChaseState")]
public class EnemyChaseState : EnemyState
{
    private ChasePhase _phase = ChasePhase.Chasing;
    private Vector3 _lastKnownPosition = Vector3.zero;

    [SerializeField, Tooltip("Time to transition from the suspicious phase to the PatrolState")]
    private float _suspiciousTime = 5f;
    [SerializeField, Tooltip("Time to transition from the confirmed phase to the FoundState")]
    private float _confirmedTime = 2f;
    private float _timer = 0f;


    public override void EnterState(EnemyController enemy)
    {
        _phase = ChasePhase.Chasing;
        _lastKnownPosition = enemy.PlayerDetection.HitPoint.Value;
        enemy.PlayerDetection.OnDetectionChanged += OnDetectionChanged;
    }

    public override void ExitState(EnemyController enemy)
    {
        enemy.PlayerDetection.OnDetectionChanged -= OnDetectionChanged;
    }


    private void OnDetectionChanged(bool wasDetected, bool wasTooClose, Vector3? hitPoint)
    {
        _lastKnownPosition = hitPoint.GetValueOrDefault(_lastKnownPosition);
    }


    public override void FixedUpdateState(EnemyController enemy)
    {
        // Move to the last known player position
        enemy.NavAgent.SetDestination(_lastKnownPosition);

        // Handle the current phase
        Action<EnemyController> handlePhase = _phase switch
        {
            ChasePhase.Chasing => HandleChasing,
            ChasePhase.Confirmed => HandleConfirmed,
            ChasePhase.Suspicious => HandleSuspicious,
            _ => throw new NotImplementedException(),
        };
        handlePhase.Invoke(enemy);
    }


    private void HandleChasing(EnemyController enemy)
    {
        if (enemy.PlayerDetection.WasDetected && enemy.PlayerDetection.WasTooClose)
        {
            _timer = _confirmedTime;
            _phase = ChasePhase.Confirmed;
        }
        else if (!enemy.NavAgent.hasPath && !enemy.NavAgent.pathPending)
        {
            _timer = _suspiciousTime;
            _phase = ChasePhase.Suspicious;
        }
    }

    private void HandleSuspicious(EnemyController enemy)
    {
        _timer -= Time.fixedDeltaTime;

        if (enemy.PlayerDetection.WasDetected)
        {
            _phase = enemy.PlayerDetection.WasTooClose ? ChasePhase.Confirmed : ChasePhase.Chasing;
            _timer = enemy.PlayerDetection.WasTooClose ? _confirmedTime : 0f;
        }
        else if (_timer <= 0f) enemy.TransitionToState(enemy.PatrolState);
    }

    private void HandleConfirmed(EnemyController enemy)
    {
        _timer -= Time.fixedDeltaTime;

        if (_timer <= 0f) enemy.TransitionToState(enemy.FoundState);
    }


    public enum ChasePhase
    {
        Chasing,
        Suspicious,
        Confirmed
    }
}
