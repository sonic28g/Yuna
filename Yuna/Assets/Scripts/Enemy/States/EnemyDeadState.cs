using UnityEngine;

[CreateAssetMenu(menuName = "Yuna/Enemy/EnemyDeadState")]
public class EnemyDeadState : EnemyState
{
    [SerializeField] private string _deadAnimationParameter = "Dead";

    private Behaviour[] _behaviours;
    private bool[] _enableds;


    public override void EnterState(EnemyController enemy)
    {
        StopNavigation(enemy);
        DisableComponents(enemy);
        DeadAnimation(enemy);
    }

    public override void ExitState(EnemyController enemy)
    {
        RestoreComponents(enemy);
        RestoreAnimation(enemy);
    }


    private void StopNavigation(EnemyController enemy)
    {
        enemy.NavAgent.isStopped = true;
        enemy.NavAgent.ResetPath();
    }


    private void DeadAnimation(EnemyController enemy)
    {
        if (enemy.Animator == null) return;
        enemy.Animator.SetBool(_deadAnimationParameter, true);
    }

    private void RestoreAnimation(EnemyController enemy)
    {
        if (enemy.Animator == null) return;
        enemy.Animator.SetBool(_deadAnimationParameter, false);
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
        _enableds = new bool[behavioursLength + 1];

        for (int i = 0; i < behavioursLength; i++)
        {
            _enableds[i] = _behaviours[i].enabled;
            _behaviours[i].enabled = false;
        }

        // Disable the collider
        if (enemy.Collider == null) return;
        _enableds[behavioursLength] = enemy.Collider.enabled;
        enemy.Collider.enabled = false;
    }

    private void RestoreComponents(EnemyController enemy)
    {
        // Restore the behaviours
        int behavioursLength = _behaviours.Length;
        for (int i = 0; i < behavioursLength; i++) _behaviours[i].enabled = _enableds[i];

        // Restore the collider
        if (enemy.Collider == null) return;
        enemy.Collider.enabled = _enableds[behavioursLength];
    }
}
