using UnityEngine;

[CreateAssetMenu(menuName = "Yuna/Enemy/EnemyDeadState")]
public class EnemyDeadState : EnemyState
{
    private bool _enable = false;

    public override void EnterState(EnemyController enemy)
    {
        enemy.NavAgent.isStopped = true;
        enemy.NavAgent.ResetPath();

        _enable = enemy.enabled;
        enemy.enabled = false;
        enemy.PlayerDetection.enabled = false;
    }

    public override void ExitState(EnemyController enemy)
    {
        enemy.enabled = _enable;
        enemy.PlayerDetection.enabled = _enable;
    }
}
