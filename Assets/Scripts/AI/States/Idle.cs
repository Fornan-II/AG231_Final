using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Idle : State
{
    //Variables
    //
    protected NavMeshAgent _agent;
    protected bool _letAutoEnd;
    protected float _autoEndTime;
    //

    //Constructors
    //
    public Idle()
    {
        _letAutoEnd = false;
    }

    public Idle(float stateEndTime)
    {
        _letAutoEnd = true;
        _autoEndTime = Time.timeSinceLevelLoad + stateEndTime;
    }
    //

    //State Behavior stuff
    //
    public override void OnEnter(GameObject gameObject)
    {
        _agent = gameObject.GetComponent<NavMeshAgent>();
        if(_agent)
        {
            _agent.isStopped = true;
            _agent.ResetPath();
        }

        _currentPhase = StatePhase.ACTIVE;
    }

    public override void ActiveBehavior(GameObject gameObject)
    {
        if(_letAutoEnd)
        {
            if(Time.timeSinceLevelLoad >= _autoEndTime)
            {
                _currentPhase = StatePhase.EXITING;
            }
        }
    }

    public override void OnExit(GameObject gameObject)
    {
        if (_agent)
        {
            _agent.isStopped = false;
        }

        _currentPhase = StatePhase.INACTIVE;
    }
    //
}
