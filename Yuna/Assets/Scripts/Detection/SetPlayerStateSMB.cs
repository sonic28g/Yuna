using UnityEngine;

public class SetPlayerStateSMB : StateMachineBehaviour
{
    private PlayerDetectionPoints _playerDetectionPoints;

    [Header("Enter State")]
    [SerializeField] private bool _changeOnEnter = false;
    [SerializeField] private PlayerState _enterState = PlayerState.Normal;

    [Header("Exit State")]
    [SerializeField] private bool _changeOnExit = false;
    [SerializeField] private PlayerState _exitState = PlayerState.Normal;


    private void CachePlayerDetectionPoints(Animator animator)
    {
        if (_playerDetectionPoints != null) return;
        _playerDetectionPoints = animator.GetComponentInParent<PlayerDetectionPoints>();
    }


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CachePlayerDetectionPoints(animator);

        if (!_changeOnEnter || _playerDetectionPoints == null) return;
        _playerDetectionPoints.SetCurrentPlayerState(_enterState);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_changeOnExit || _playerDetectionPoints == null) return;
        _playerDetectionPoints.SetCurrentPlayerState(_exitState);
    }
}