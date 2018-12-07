using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateMachine))]
public class AIBrain : MonoBehaviour
{
    protected StateMachine _stateMachine;

    private void Start()
    {
        _stateMachine = GetComponent<StateMachine>();
    }

    #region Debug Manual State Selection
    public Transform[] PatrolPoints;
    public Transform thingToInvestigate;

    public enum State
    {
        IDLE, PATROL, INVESTIGATE
    }
    public State DesiredState;

    private void OnValidate()
    {
        if(!_stateMachine) { return; }

        if (DesiredState == State.IDLE)
        {
            _stateMachine.CurrentState = new Idle();
            _stateMachine.ForceNextState();
        }
        if (DesiredState == State.PATROL)
        {
            List<Vector3> points = new List<Vector3>();
            foreach (Transform t in PatrolPoints)
            {
                points.Add(t.position);
            }
            _stateMachine.CurrentState = new Patrol(points);
            _stateMachine.ForceNextState();
        }
        if(DesiredState == State.INVESTIGATE && thingToInvestigate)
        {
            _stateMachine.CurrentState = new Investigate(thingToInvestigate.position, 0);
            _stateMachine.ForceNextState();
        }
    }
    #endregion
}
