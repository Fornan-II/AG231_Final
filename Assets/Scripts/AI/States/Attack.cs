﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Attack : State
{
    //Variables
    //
    protected Transform _target;
    protected Vector3 _lastKnownPosition;
    protected NavMeshAgent _agent;
    protected float _atLastKnownPositionRange;
    public RobotArmLasers weapon;
    public float attackRange;
    //

    //Constructors
    //
    public Attack(Transform target, RobotArmLasers wpn = null, float atkRange = 10.0f)
    {
        _target = target;
        _lastKnownPosition = target.position;
        attackRange = atkRange;
        weapon = wpn;
    }
    //

    //State Behavior stuff
    //
    public override void OnEnter(GameObject gameObject)
    {
        _agent = gameObject.GetComponent<NavMeshAgent>();
        if(_agent)
        {
            _agent.isStopped = false;
            _agent.ResetPath();
            _agent.SetDestination(_lastKnownPosition);
            _currentPhase = StatePhase.ACTIVE;
        }
        else
        {
            _currentPhase = StatePhase.EXITING;
        }
    }

    public override void ActiveBehavior(GameObject gameObject)
    {
        if(_agent)
        {
            _agent.isStopped = false;
            bool hasNotAimedAtTarget = true;

            Vector3 vectorToTarget = _target.position - gameObject.transform.position;
            float vectorToTargetMagnitude = vectorToTarget.magnitude;
            Ray lineOfSightRay = new Ray(gameObject.transform.position, vectorToTarget);
            if(1 >= Physics.RaycastNonAlloc(lineOfSightRay, null, vectorToTargetMagnitude, AIBrain.LineOfSightBlocking, QueryTriggerInteraction.Ignore))
            {
                _lastKnownPosition = _target.position;
                if (weapon)
                {
                    weapon.AimAtPoint(_lastKnownPosition);
                    hasNotAimedAtTarget = false;
                }
                if (vectorToTargetMagnitude <= attackRange && weapon)
                {
                    _agent.isStopped = true;
                    weapon.Attack();
                }
            }
            else if(!_agent.pathPending && _agent.remainingDistance <= _atLastKnownPositionRange)
            {
                _currentPhase = StatePhase.EXITING;
            }

            if(hasNotAimedAtTarget && weapon)
            {
                weapon.ResetAiming();
            }
        }
        else
        {
            _currentPhase = StatePhase.EXITING;
        }
    }

    public override void OnExit(GameObject gameObject)
    {
        if (_agent)
        {
            _agent.ResetPath();
            _agent.isStopped = false;
        }

        _currentPhase = StatePhase.INACTIVE;
    }
    //
}