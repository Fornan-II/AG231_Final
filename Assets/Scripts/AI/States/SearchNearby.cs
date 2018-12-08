using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SearchNearby : State
{
    //Variables
    //
    protected NavMeshAgent _agent;
    protected Vector3 _pointOfInterest;
    protected bool _atSearchPoint = false;
    protected float _searchAtPointEndTime;
    public float initialSearchRadius = 5.0f;
    public float currentSearchRadius;
    public float searchRadiusIncrease = 1.5f;
    public float searchEndAtTime;
    public float DistanceForFindingSearchPoint = 1.0f;
    public float[] searchAtPointTimeRange = new float[] { 1.0f, 5.0f };
    //

    //Constructors
    //
    public SearchNearby(Vector3 point)
    {
        _pointOfInterest = point;

        searchEndAtTime = Time.timeSinceLevelLoad + Random.Range(20.0f, 60.0f);
    }

    public SearchNearby(Vector3 point, float searchTimeMinimum, float searchTimeMaximum)
    {
        _pointOfInterest = point;

        searchEndAtTime = Time.timeSinceLevelLoad + Random.Range(searchTimeMinimum, searchTimeMaximum);
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
            currentSearchRadius = initialSearchRadius;
            _agent.SetDestination(GetPointToSearch(_pointOfInterest, currentSearchRadius));
            _currentPhase = StatePhase.ACTIVE;
        }
        else
        {
            _currentPhase = StatePhase.EXITING;
        }
    }

    public override void ActiveBehavior(GameObject gameObject)
    {
        if(_agent && Time.timeSinceLevelLoad < searchEndAtTime)
        {
            if(_atSearchPoint)
            {
                if(Time.timeSinceLevelLoad >= _searchAtPointEndTime)
                {
                    _agent.isStopped = false;
                    _agent.ResetPath();
                    _agent.SetDestination(GetPointToSearch(_pointOfInterest, currentSearchRadius));
                    _atSearchPoint = false;
                }
            }
            else if(!_agent.pathPending && _agent.remainingDistance < DistanceForFindingSearchPoint)
            {
                _atSearchPoint = true;
                _agent.isStopped = true;
                currentSearchRadius += searchRadiusIncrease;
                _searchAtPointEndTime = Time.timeSinceLevelLoad + Random.Range(searchAtPointTimeRange[0], searchAtPointTimeRange[1]);
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
    //
    protected Vector3 GetPointToSearch(Vector3 searchCenter, float searchRadius, int numTrys = 7)
    {
        Vector2 searchOffset;
        Vector3 searchPoint;

        for(int tryCount = 1; tryCount <= numTrys; tryCount++)
        {
            searchOffset = Random.insideUnitCircle * searchRadius;
            searchPoint = searchCenter + new Vector3(searchOffset.x, 0.0f, searchOffset.y);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(searchPoint, out hit, searchRadius, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        return searchCenter;
    }
    //
}
