using UnityEngine;

[CreateAssetMenu(menuName = "Yuna/Enemy/EnemyFoundState")]
public class EnemyFoundState : EnemyState
{
    private Behaviour[] _behaviours;
    private bool[] _enableds;


    public override void EnterState(EnemyController enemy)
    {
        if (CheckpointManager.Instance != null) CheckpointManager.Instance.RespawnPlayer();

        StopNavigation(enemy);
        DisableComponents(enemy);
    }

    public override void ExitState(EnemyController enemy) => RestoreComponents(enemy);


    private void StopNavigation(EnemyController enemy)
    {
        enemy.NavAgent.isStopped = true;
        enemy.NavAgent.ResetPath();
    }


    private void DisableComponents(EnemyController enemy)
    {
        // Behaviours to disable
        _behaviours = new Behaviour[] {
            enemy,
            enemy.NavAgent, enemy.PlayerDetection,
            enemy.SoundDetection,
            enemy.EnemyHealth
            // ...
        };

        // Store the enabled value and disable the behaviours
        int behavioursLength = _behaviours.Length;
        _enableds = new bool[behavioursLength];

        for (int i = 0; i < behavioursLength; i++)
        {
            // Check if the behaviour is null
            if (_behaviours[i] == null) continue;

            _enableds[i] = _behaviours[i].enabled;
            _behaviours[i].enabled = false;
        }
    }

    private void RestoreComponents(EnemyController _)
    {
        // Restore the behaviours
        int behavioursLength = _behaviours.Length;
        for (int i = 0; i < behavioursLength; i++)
        {
            // Check if the behaviour is null
            if (_behaviours[i] == null) continue;

            _behaviours[i].enabled = _enableds[i];
        }
    }
}
