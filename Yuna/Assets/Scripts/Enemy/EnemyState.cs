using UnityEngine;

public abstract class EnemyState : ScriptableObject
{
    public virtual void InitState(EnemyController enemy) {}

    public virtual void EnterState(EnemyController enemy) {}

    public virtual void UpdateState(EnemyController enemy) {}
    public virtual void FixedUpdateState(EnemyController enemy) {}

    public virtual void ExitState(EnemyController enemy) {}
}