using UnityEngine;

[CreateAssetMenu(menuName = "Yuna/Enemy/EnemyFoundState")]
public class EnemyFoundState : EnemyState
{
    private Behaviour[] _behaviours;
    private bool[] _enableds;

    public override void EnterState(EnemyController enemy)
    {
        CheckpointManager.Instance.RespawnPlayer();

        
        enemy.NavAgent.isStopped = true;
        enemy.NavAgent.ResetPath();

        // Behaviours to disable
        _behaviours = new Behaviour[] {
            enemy,
            enemy.NavAgent, enemy.PlayerDetection,
            enemy.SoundDetection,
            // ...
        };

        // Store the enabled value and disable the behaviours
        _enableds = new bool[_behaviours.Length];
        for (int i = 0; i < _behaviours.Length; i++)
        {
            _enableds[i] = _behaviours[i].enabled;
            _behaviours[i].enabled = false;
        }
    }

    public override void ExitState(EnemyController enemy)
    {
        for (int i = 0; i < _behaviours.Length; i++) _behaviours[i].enabled = _enableds[i];
    }
}
