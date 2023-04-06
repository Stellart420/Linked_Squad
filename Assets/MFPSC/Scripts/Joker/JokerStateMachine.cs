using System;
using System.Linq;
using Unity.AI.Navigation.Samples;
using UnityEngine;
using UnityEngine.AI;

public class JokerStateMachine : AIStateMachineBase<JokerStateBase>
{
    [field: SerializeField] public NavMeshAgent Agent { get; private set; }
    [SerializeField] private ChaseState _chaseState;
    
    public JokerAnimationController AnimationsController { get; private set; }

    public void Init(JokerAnimationController controller)
    {
        AnimationsController = controller;
        _states.ForEach(state => state.Init(this));
        base.Init();
    }

    public void Alert()
    {
        SetState(_chaseState);
    }

    public virtual JokerStateBase GetState(Type stateType)
    {
        return _states.FirstOrDefault(state => stateType == state.GetType());
    }
}
