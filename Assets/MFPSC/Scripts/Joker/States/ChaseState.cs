using Unity.AI.Navigation.Samples;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class ChaseState : JokerStateBase
{
    [SerializeField] private float _chaseSpeed = 4f;
    [SerializeField] private FieldOfView _fov;
    [SerializeField] private float _ifLeftTargetFollowTime = 1;

    private NavMeshAgent _agent;
    private Vector3 _lastKnownPosition;
    private float _timeSinceLastSawTarget;
    private Transform _target;
    private float _followTime = 0f;

    public override void Init(JokerStateMachine stateMachine)
    {
        base.Init(stateMachine);
        _agent = StateMachine.Agent;
    }

    public override void Enter()
    {
        base.Enter();
        _agent.speed = _chaseSpeed;
        StateMachine.AnimationsController.Running();
        _target = _fov.Target;
        _lastKnownPosition = _target.position;
        _timeSinceLastSawTarget = 0;
        FollowTarget();
        Debug.Log($"Enter: {this.name}");
    }
    public override void Tick()
    {
        if (_fov.Target == null)
        {
            if (_target != null)
            {
                if (_followTime >= _ifLeftTargetFollowTime)
                {
                    FollowLastKnownPosition();
                    Debug.Log("Go to Last Pos");
                    _target = null;
                }
                else
                {
                    Debug.Log("Follow Last Target");
                    _followTime += Time.deltaTime;
                    _lastKnownPosition = _target.position;
                }
            }
            else
            {
                if (StateMachine.Agent.remainingDistance <= 0.1f)
                {
                    var lookAroundState = StateMachine.GetState(typeof(LookAroundState));
                    StateMachine.SetState(lookAroundState);
                    return;
                }
            }
        }
        else
        {
            _target = _fov.Target;
            _followTime = 0;
            _timeSinceLastSawTarget = 0;
            _lastKnownPosition = _target.position;
            FollowTarget();
            Debug.Log("See Target");
        }
        
    }

    public override void Exit()
    {
        base.Exit();
        _target = null;
        Debug.Log($"Exit: {this.name}");
    }


    private void FollowTarget()
    {
        NavMeshHit hit = new NavMeshHit();
        StateMachine.Agent.ResetPath();
        NavMesh.SamplePosition(_target.position, out hit, float.MaxValue, StateMachine.Agent.areaMask);
        StateMachine.Agent.SetDestination(hit.position);
    }

    private void FollowLastKnownPosition()
    {
        StateMachine.Agent.SetDestination(_lastKnownPosition);
    }
}
