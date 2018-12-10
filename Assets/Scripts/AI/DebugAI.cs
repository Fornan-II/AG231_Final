using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugAI : MonoBehaviour
{
    public Transform[] PatrolPoints;
    public Transform thingToInvestigate;
    public Vector2 searchTime = new Vector2(7.0f, 20.0f);
    public RobotArmLasers Lasers;

    public StateMachine myStateMachine;

    public enum State
    {
        IDLE, PATROL, INVESTIGATE, SEARCHNEARBY, ATTACK
    }
    public State DesiredState;

    private void OnValidate()
    {
        if (!myStateMachine) { return; }

        if (DesiredState == State.IDLE)
        {
            myStateMachine.CurrentState = new Idle();
            myStateMachine.ForceNextState();
        }
        if (DesiredState == State.PATROL)
        {
            myStateMachine.CurrentState = new Patrol(PatrolPoints);
            myStateMachine.ForceNextState();
        }
        if (DesiredState == State.INVESTIGATE && thingToInvestigate)
        {
            myStateMachine.CurrentState = new Investigate(thingToInvestigate.position, 0);
            myStateMachine.ForceNextState();
        }
        if (DesiredState == State.SEARCHNEARBY && thingToInvestigate)
        {
            myStateMachine.CurrentState = new SearchNearby(thingToInvestigate.position, searchTime.x, searchTime.y);
            myStateMachine.ForceNextState();
        }
        if (DesiredState == State.ATTACK && thingToInvestigate)
        {
            myStateMachine.CurrentState = new Attack(thingToInvestigate, Lasers);
            myStateMachine.ForceNextState();
        }
    }
}
