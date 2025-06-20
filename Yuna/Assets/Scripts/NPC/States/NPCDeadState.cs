using UnityEngine;

[CreateAssetMenu(menuName = "Yuna/NPC/NPCDeadState")]
public class NPCDeadState : NPCState
{
    private Behaviour[] _behaviours;
    private bool[] _enableds;


    public override void EnterState(NPCController npc)
    {
        if (CheckpointManager.Instance != null) CheckpointManager.Instance.RespawnPlayer();

        StopNavigation(npc);
        DisableComponents(npc);
    }

    public override void ExitState(NPCController npc) => RestoreComponents(npc);


    private void StopNavigation(NPCController npc)
    {
        npc.NavAgent.isStopped = true;
        npc.NavAgent.ResetPath();
    }


    private void DisableComponents(NPCController npc)
    {
        // Behaviours to disable
        _behaviours = new Behaviour[] {
            npc,
            npc.NavAgent,
            npc.EnemyHealth
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

    private void RestoreComponents(NPCController _)
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
