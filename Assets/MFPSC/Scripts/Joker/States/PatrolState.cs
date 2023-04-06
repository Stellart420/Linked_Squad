using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PatrolState : JokerStateBase
{
    [SerializeField] private float _patrolSpeed = 3f;
    [SerializeField] private float _stayOnPointTime = 3f;
    
    [Header("TEST")]
    [SerializeField] private Transform _pointOutSideDoor;

    private NavMeshAgent _agent;
    private Vector3 _currentPoint;
    private float _stayTime = 0;

    public override void Init(JokerStateMachine stateMachine)
    {
        base.Init(stateMachine);
        _agent = stateMachine.Agent;
    }

    public override void Enter()
    {
        GetNextPoint();
        _agent.speed = _patrolSpeed;
        _agent.SetDestination(_currentPoint);
        Debug.Log($"Enter: {this.name}");
    }

    public override void Exit()
    {
        _agent.SetDestination(_agent.transform.position);
        _stayTime = 0;
        Debug.Log($"Exit: {this.name}");
    }

    public override void Tick()
    {
        if (_agent.remainingDistance <= 0)
        {
            StateMachine.AnimationsController.Idle();
            if (_stayTime >= _stayOnPointTime)
            {
                GetNextPoint();
                _agent.SetDestination(_currentPoint);
                _stayTime = 0;
            }
            else
            {
                _stayTime += Time.deltaTime;
            }
        }
        else
        {
            StateMachine.AnimationsController.Walking();
        }
    }

    private void GetNextPoint()
    {
        NavMeshPath navMeshPath = new NavMeshPath();
        var canRich = false;
        while (!canRich)
        {
            var point = StateMachine.GetRandomNavMeshPoint();
            if (NavMesh.CalculatePath(_agent.transform.position, point, _agent.areaMask, navMeshPath))
            {
                _currentPoint = point;
                canRich = true;
            }
        }
    }

    [Button()]
    public void SetPointOutsideTheDoor()
    {
        NavMeshPath navMeshPath = new NavMeshPath();
        var point = _pointOutSideDoor.position;
        if (NavMesh.CalculatePath(_agent.transform.position, point, _agent.areaMask, navMeshPath))
        {
            _currentPoint = _pointOutSideDoor.position;
            _agent.SetDestination(_currentPoint);
        }
    }

    private void OnDrawGizmos()
    {
        if (_agent == null) return;
            
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_agent.transform.position, _currentPoint);
    }
}
