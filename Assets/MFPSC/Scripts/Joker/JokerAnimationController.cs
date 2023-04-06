using System;
using Unity.AI.Navigation.Samples;
using UnityEngine;

public class JokerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private AgentLinkMover _agentLinkMover;

    public static readonly int WALKING_HASH = Animator.StringToHash("Walking");
    public static readonly int RUNNING_HASH = Animator.StringToHash("Running");
    public static readonly int FALL_HASH = Animator.StringToHash("Fall");
    public static readonly int LANDED_HASH = Animator.StringToHash("Landed");

    public void Awake()
    {
        _agentLinkMover.OnLinkStart += Jump;
        _agentLinkMover.OnLinkEnd += Landed;
    }

    private void OnDestroy()
    {
       _agentLinkMover.OnLinkStart -= Jump;
        _agentLinkMover.OnLinkEnd -= Landed;
    }

    public void Idle()
    {
        _animator.SetBool(WALKING_HASH, false);
        _animator.SetBool(RUNNING_HASH, false);
    }

    public void Walking()
    {
        _animator.SetBool(RUNNING_HASH, false);
        _animator.SetBool(WALKING_HASH, true);
    }

    public void Running()
    {
        _animator.SetBool(WALKING_HASH, false);
        _animator.SetBool(RUNNING_HASH, true);
    }

    public void Jump()
    {
        _animator.SetTrigger(FALL_HASH);
    }

    public void Landed()
    {
        _animator.SetTrigger(LANDED_HASH);
    }
}
