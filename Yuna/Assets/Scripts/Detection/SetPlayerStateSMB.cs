using UnityEngine;

public class SetPlayerStateSMB : StateMachineBehaviour
{
    private PlayerDetectionPoints _playerDetectionPoints;

    [SerializeField] private PlayerState? _enterState;
    [SerializeField] private PlayerState? _exitState;


    private void CachePlayerDetectionPoints(Animator animator)
    {
        if (_playerDetectionPoints != null) return;
        _playerDetectionPoints = animator.GetComponentInParent<PlayerDetectionPoints>();
    }


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CachePlayerDetectionPoints(animator);

        if (!_enterState.HasValue || _playerDetectionPoints == null) return;
        _playerDetectionPoints.SetCurrentPlayerState(_enterState.Value);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_exitState.HasValue || _playerDetectionPoints == null) return;
        _playerDetectionPoints.SetCurrentPlayerState(_exitState.Value);
    }
}