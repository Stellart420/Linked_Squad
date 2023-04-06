using System;
using UnityEngine;

public class JokerStateBase : StateBase
{
    public JokerStateMachine StateMachine { get; private set; }
    
    public virtual void Init(JokerStateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }

    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void Tick()
    {
    }
}
