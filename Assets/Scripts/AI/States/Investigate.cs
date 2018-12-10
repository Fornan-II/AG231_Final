using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Investigate : State
{
    //Variables
    //
    protected NavMeshAgent _agent;
    protected Vector3 _pointOfInterest;
    protected int _pointPriority;
    protected bool _investigationPointReached;
    protected float _investigationEndAtTime;
    public float DistanceForFindingInvestigationPoint = 1.0f;
    public float[] InvestigationTimeRange;
    //

    //Constructors
    //
    public Investigate(Vector3 point, int priority)
    {
        _pointOfInterest = point;
        _pointPriority = priority;

        InvestigationTimeRange = new float[] { 1.0f, 5.0f };
    }
    public Investigate(Vector3 point, int priority, float investigationTimeMinimum, float investigationTimeMaximum)
    {
        _pointOfInterest = point;
        _pointPriority = priority;

        InvestigationTimeRange = new float[] { investigationTimeMinimum, investigationTimeMaximum };
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
            _agent.SetDestination(_pointOfInterest);
            _investigationPointReached = false;
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
            if (_investigationPointReached && (Time.timeSinceLevelLoad >= _investigationEndAtTime))
            {
                _currentPhase = StatePhase.EXITING;
            }
            else if (!_agent.pathPending && !_investigationPointReached && _agent.remainingDistance <= DistanceForFindingInvestigationPoint)
            {
                _agent.isStopped = true;
                _investigationPointReached = true;
                _investigationEndAtTime = Time.timeSinceLevelLoad + Random.Range(InvestigationTimeRange[0], InvestigationTimeRange[1]);
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

        AIBrain brain = gameObject.GetComponent<AIBrain>();
        if(brain)
        {
            brain.QueueState(new SearchNearby(_pointOfInterest, 5.0f, 20.0f));
        }

        _currentPhase = StatePhase.INACTIVE;
    }
    //

    //Utility Methods
    //
    public bool SetNewPointOfInterest(Vector3 point, int priority)
    {
        if(priority >= _pointPriority)
        {
            _pointOfInterest = point;
            _currentPhase = StatePhase.ENTERING;
            return true;
        }

        return false;
    }
    //
}
