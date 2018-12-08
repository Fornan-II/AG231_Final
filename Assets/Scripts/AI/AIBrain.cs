using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateMachine))]
public class AIBrain : MonoBehaviour
{
    public static LayerMask LineOfSightBlocking = Physics.AllLayers;
    protected StateMachine _stateMachine;

    private void Start()
    {
        _stateMachine = GetComponent<StateMachine>();
    }

    #region Debug Manual State Selection
    public Transform[] PatrolPoints;
    public Transform thingToInvestigate;
    public Vector2 searchTime = new Vector2(7.0f, 20.0f);
    public RobotArmLasers Lasers;

    public enum State
    {
        IDLE, PATROL, INVESTIGATE, SEARCHNEARBY, ATTACK
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
        if(DesiredState == State.SEARCHNEARBY && thingToInvestigate)
        {
            _stateMachine.CurrentState = new SearchNearby(thingToInvestigate.position, searchTime.x, searchTime.y);
            _stateMachine.ForceNextState();
        }
        if(DesiredState == State.ATTACK && thingToInvestigate)
        {
            _stateMachine.CurrentState = new Attack(thingToInvestigate, Lasers);
            _stateMachine.ForceNextState();
        }
    }
    #endregion
}
