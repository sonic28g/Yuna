using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Yuna/Enemy/EnemyDeadState")]
public class EnemyDeadState : EnemyState
{
    [SerializeField] private string _deadAnimationParameter = "Dead";

    private Behaviour[] _behaviours;
    private bool[] _enableds;
    private Color _outlineColor;


    public override void EnterState(EnemyController enemy)
    {
        StopNavigation(enemy);
        DisableComponents(enemy);
        DeadAnimation(enemy);
        NoOutline(enemy);
    }

    public override void ExitState(EnemyController enemy)
    {
        RestoreComponents(enemy);
        RestoreAnimation(enemy);
        RestoreOutline(enemy);
    }


    private void StopNavigation(EnemyController enemy)
    {
        enemy.NavAgent.isStopped = true;
        enemy.NavAgent.ResetPath();
    }


    private void DeadAnimation(EnemyController enemy)
    {
        if (!HaveAnimationParameter(enemy)) return;
        enemy.Animator.SetBool(_deadAnimationParameter, true);
    }

    private void RestoreAnimation(EnemyController enemy)
    {
        if (!HaveAnimationParameter(enemy)) return;
        enemy.Animator.SetBool(_deadAnimationParameter, false);
    }

    private bool HaveAnimationParameter(EnemyController enemy)
    {
        // Check if have a animator
        if (enemy.Animator == null) return false;

        // Check if have a parameter with the same name
        AnimatorControllerParameter parameter = enemy.Animator.parameters.FirstOrDefault(p => p.name == _deadAnimationParameter);
        if (parameter == null) return false;

        // Check if the parameter is a bool type
        bool isBool = parameter.type == AnimatorControllerParameterType.Bool;
        if (!isBool) return false;

        return true;
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

        int behavioursLength = _behaviours.Length;
        int collidersLength = enemy.Colliders.Length;
        _enableds = new bool[behavioursLength + collidersLength];

        // Store enabled values and disable the behaviours
        for (int i = 0; i < behavioursLength; i++)
        {
            // Check if the behaviour is null
            Behaviour behaviour = _behaviours[i];
            if (behaviour == null) continue;

            _enableds[i] = behaviour.enabled;
            behaviour.enabled = false;
        }

        // Store enabled values and disable the colliders
        for (int i = 0; i < collidersLength; i++)
        {
            // Check if the collider is null
            Collider collider = enemy.Colliders[i];
            if (collider == null) continue;

            _enableds[behavioursLength + i] = collider.enabled;
            collider.enabled = false;
        }
    }

    private void RestoreComponents(EnemyController enemy)
    {
        // Restore the behaviours
        int behavioursLength = _behaviours.Length;
        for (int i = 0; i < behavioursLength; i++)
        {
            // Check if the behaviour is null
            Behaviour behaviour = _behaviours[i];
            if (behaviour == null) continue;

            behaviour.enabled = _enableds[i];
        }

        // Restore the colliders
        int collidersLength = enemy.Colliders.Length;
        for (int i = 0; i < collidersLength; i++)
        {
            // Check if the collider is null
            Collider collider = enemy.Colliders[i];
            if (collider == null) continue;

            collider.enabled = _enableds[behavioursLength + i];
        }
    }


    private void NoOutline(EnemyController enemy)
    {
        if (enemy.Outline == null) return;

        _outlineColor = enemy.Outline.OutlineColor;
        enemy.Outline.OutlineColor = Color.clear;
    }

    private void RestoreOutline(EnemyController enemy)
    {
        if (enemy.Outline == null) return;

        enemy.Outline.OutlineColor = _outlineColor;
    }
}
