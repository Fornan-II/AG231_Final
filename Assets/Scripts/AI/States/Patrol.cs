using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : State
{
    //Variables
    //
    protected List<Vector3> _patrolPoints;
    protected Vector3 _targetPoint;
    protected NavMeshAgent _agent;
    public float DistanceForRandomPoints;
    public float DistanceForRecalculatingTargetPoint = 0.3f;
    //

    //Constructors
    //
    public Patrol(List<Vector3> patrolPoints, float distForRanPnts = 21.0f)
    {
        if(patrolPoints != null)
        {
            _patrolPoints = patrolPoints;
        }
        else
        {
            _patrolPoints = new List<Vector3>();
        }

        DistanceForRandomPoints = distForRanPnts;
    }
    public Patrol(List<Transform> patrolPoints, float distForRanPnts = 21.0f)
    {
        _patrolPoints = new List<Vector3>();
        foreach(Transform t in patrolPoints)
        {
            _patrolPoints.Add(t.position);
        }

        DistanceForRandomPoints = distForRanPnts;
    }
    //

    //State Behavior stuffs
    //
    public override void OnEnter(GameObject gameObject)
    {
        if(_patrolPoints.Count <= 0)
        {
            _currentPhase = StatePhase.EXITING;

        }

        _agent = gameObject.GetComponent<NavMeshAgent>();
        if(_agent)
        {
            _agent.isStopped = false;
            _agent.ResetPath();
            _targetPoint = SelectTargetPoint(gameObject.transform.position);
            _agent.SetDestination(_targetPoint);
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
            if(!_agent.pathPending && _agent.remainingDistance < DistanceForRecalculatingTargetPoint)
            {
                _targetPoint = SelectTargetPoint(gameObject.transform.position);
                _agent.SetDestination(_targetPoint);
            }
        }
        else
        {
            _currentPhase = StatePhase.EXITING;
        }
    }

    public override void OnExit(GameObject gameObject)
    {
        if(_agent)
        {
            _agent.ResetPath();
            _agent.isStopped = false;
        }

        _currentPhase = StatePhase.INACTIVE;
    }
    //

    //Utility Methods
    protected Vector3 SelectTargetPoint(Vector3 currentPosition)
    {
        Vector3 closestPoint = currentPosition;
        float closestPointDistance = 100.0f;
        List<Vector3> validPointsForRandomSelection = new List<Vector3>();

        foreach(Vector3 point in _patrolPoints)
        {
            if(point != _targetPoint)
            {
                float dist = Vector3.Distance(point, currentPosition);
                if (dist < closestPointDistance)
                {
                    closestPoint = point;
                }

                if (dist <= DistanceForRandomPoints)
                {
                    validPointsForRandomSelection.Add(point);
                }
            }
        }

        if(validPointsForRandomSelection.Count <= 0)
        {
            return closestPoint;
        }
        else
        {
            int i = Random.Range(0, validPointsForRandomSelection.Count);
            return validPointsForRandomSelection[i];
        }
    }
    //
}
