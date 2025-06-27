using UnityEngine;

public abstract class NPCState : ScriptableObject
{
    public virtual void InitState(NPCController npc) {}

    public virtual void EnterState(NPCController npc) {}

    public virtual void UpdateState(NPCController npc) {}
    public virtual void FixedUpdateState(NPCController npc) {}

    public virtual void ExitState(NPCController npc) {}
}