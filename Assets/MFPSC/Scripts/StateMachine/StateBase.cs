using UnityEngine;

public abstract class StateBase : MonoBehaviour, IAIState
{
    public abstract void Enter();

    public abstract void Exit();

    public abstract void Tick();
}