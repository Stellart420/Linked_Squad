using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation.Samples;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;


public class JokerController : MonoBehaviour
{
    [SerializeField] private JokerStateMachine _stateMachine;
    [SerializeField] private JokerAnimationController _animationController;
    [SerializeField] private FieldOfView _fieldOfView;

    private void Awake()
    {
        _stateMachine.Init(_animationController);
        _fieldOfView.TargetFinded += FOV_TargetFinded;
    }

    private void FOV_TargetFinded(Transform target)
    {
        if (target != null)
        {
            _stateMachine.Alert();
        }
    }
}
