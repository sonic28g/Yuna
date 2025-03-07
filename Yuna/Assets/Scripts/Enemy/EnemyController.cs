using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyState _currentState;

    // States
    // ...


    private void Awake()
    {
        InitializeStates();
    }

    private void Start()
    {
        // TransitionToState(...);
    }


    public void TransitionToState(EnemyState state)
    {
        if (_currentState != null) _currentState.ExitState(this);
        _currentState = state;
        _currentState.EnterState(this);
    }


    private void InitializeStates()
    {
        // ...
    }

    private void Update()
    {
        if (_currentState != null) _currentState.UpdateState(this);
    }

    private void FixedUpdate()
    {
        if (_currentState != null) _currentState.FixedUpdateState(this);
    }
}
