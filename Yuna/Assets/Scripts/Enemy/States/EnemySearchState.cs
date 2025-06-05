using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Yuna/Enemy/EnemySearchState")]
public class EnemySearchState : EnemyState
{
    private SearchPhase _phase = SearchPhase.Searching;

    [SerializeField, Tooltip("Time to transition from the suspicious phase to the PatrolState")]
    private float _suspiciousTime = 5f;
    private float _timer = 0f;

    [SerializeField, Tooltip("Speed of the enemy during the search phase")]
    private float _searchSpeed = 2f;


    public override void EnterState(EnemyController enemy)
    {
        _phase = SearchPhase.Searching;
        enemy.PlayerDetection.SetDetectionMode(PlayerDetection.DetectionMode.Search);

        // Set the search speed
        enemy.NavAgent.speed = _searchSpeed;
    }

    public override void ExitState(EnemyController enemy)
    {
        enemy.PlayerDetection.SetDetectionMode(PlayerDetection.DetectionMode.Normal);
    }


    public override void FixedUpdateState(EnemyController enemy)
    {
        // Transition to other states
        if (enemy.PlayerDetection.WasDetected)
        {
            enemy.TransitionToState(enemy.ChaseState);
            return;
        }
        // ...

        // Move to the last sound position
        Vector3 destination = enemy.SoundDetection.LastSoundPosition.GetValueOrDefault(enemy.NavAgent.destination);
        enemy.NavAgent.SetDestination(destination);

        // Handle the current phase
        Action<EnemyController> handlePhase = _phase switch
        {
            SearchPhase.Searching => HandleChasing,
            SearchPhase.Suspicious => HandleSuspicious,
            _ => throw new NotImplementedException(),
        };
        handlePhase.Invoke(enemy);
    }


    private void HandleChasing(EnemyController enemy)
    {
        if (enemy.NavAgent.hasPath || enemy.NavAgent.pathPending) return;

        _timer = _suspiciousTime;
        _phase = SearchPhase.Suspicious;
        enemy.SoundDetection.CheckedLastPosition();
    }

    private void HandleSuspicious(EnemyController enemy)
    {
        _timer -= Time.fixedDeltaTime;

        if (enemy.SoundDetection.WasSoundDetected) _phase = SearchPhase.Searching;
        else if (_timer <= 0f) enemy.TransitionToState(enemy.PatrolState);
    }


    public enum SearchPhase
    {
        Searching,
        Suspicious
    }
}
