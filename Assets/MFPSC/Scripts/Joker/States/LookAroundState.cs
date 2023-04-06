using UnityEngine;
using UnityEngine.AI;

public class LookAroundState : JokerStateBase
{
    [SerializeField] private float _turnSpeed = 20f;
    [SerializeField] private float _timeToLook = 2f;

    private NavMeshAgent _agent;
    private float _lookTimer;
    private int _lookDirection;

    public override void Init(JokerStateMachine stateMachine)
    {
        base.Init(stateMachine);
        _agent = stateMachine.Agent;
    }

    public override void Enter()
    {
        base.Enter();

        StateMachine.AnimationsController.Idle();
        _lookDirection = -1;
        _lookTimer = _timeToLook;
        Debug.Log($"Enter: {this.name}");
    }

    public override void Tick()
    {
        base.Tick();

        _agent.transform.Rotate(Vector3.up, _lookDirection * _turnSpeed * Time.deltaTime);

        _lookTimer -= Time.deltaTime;


        if (_lookDirection == 1 && _lookTimer <= 0)
        {
            StateMachine.SetState(StateMachine.GetState(typeof(PatrolState)));
        }

        if (_lookTimer <= 0)
        {
            _lookDirection *= -1;
            _lookTimer = _timeToLook;
        }
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log($"Exit: {this.name}");
    }
}
