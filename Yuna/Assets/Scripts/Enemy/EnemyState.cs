using UnityEngine;

public abstract class EnemyState : ScriptableObject
{
    public virtual void InitState(EnemyController player) {}

    public virtual void EnterState(EnemyController player) {}

    public virtual void UpdateState(EnemyController player) {}
    public virtual void FixedUpdateState(EnemyController player) {}

    public virtual void ExitState(EnemyController player) {}
}