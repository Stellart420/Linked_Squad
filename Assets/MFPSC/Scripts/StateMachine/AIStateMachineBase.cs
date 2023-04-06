using System;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachineBase<T> : MonoBehaviour where T : StateBase
{
    [SerializeField] private T _initialState;
    [SerializeField] internal List<T> _states = new List<T>();

    private StateBase _currentState;
    internal readonly Dictionary<IAIState, T> states = new Dictionary<IAIState, T>();

    public virtual void Init()
    {
        if (_states != null && _states.Count > 0)
        {
            _states.ForEach(state =>
            {
                states.Add(state, state);
            });
        }

        SetState(_initialState);
    }

    private void Update()
    {
        if (_currentState != null)
            _currentState.Tick();
    }

    public void SetState(T newState)
    {
        if (_currentState != newState)
        {
            if (_currentState != null)
            {
                _currentState.Exit();
            }

            _currentState = newState;

            if (_currentState != null)
            {
                _currentState.Enter();
            }
        }
    }
}
