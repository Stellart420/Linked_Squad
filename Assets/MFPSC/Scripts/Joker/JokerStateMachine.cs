using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

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

    public Vector3 GetRandomNavMeshPoint()
    {
        var navMesh = NavMesh.CalculateTriangulation();
        int index = Random.Range(0, navMesh.vertices.Length);
        return navMesh.vertices[index];
    }
}
