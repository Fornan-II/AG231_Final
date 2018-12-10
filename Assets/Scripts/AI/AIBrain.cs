using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateMachine))]
public class AIBrain : MonoBehaviour
{
    public static LayerMask LineOfSightBlocking = Physics.AllLayers;
    public GameObject Player;
    public float sightRange = 7.0f;
    public float fieldOfView = 120.0f;
    public Transform eyeTransform;
    public float AIThinkRate = 0.03f;
    public Transform[] PatrolPoints;
    public RobotArmLasers Lasers;

    protected StateMachine _stateMachine;
    protected float _AIThinkTimer = 0.0f;

    public enum AIState
    {
        NONE,
        IDLE,
        PATROL,
        INVESTIGATE,
        SEARCHNEARBY,
        ATTACK
    }
    protected Dictionary<System.Type, AIState> stateTypes = new Dictionary<System.Type, AIState>()
    {
        {typeof(Idle), AIState.IDLE},
        {typeof(Patrol), AIState.PATROL},
        {typeof(Investigate), AIState.INVESTIGATE},
        {typeof(SearchNearby), AIState.SEARCHNEARBY},
        {typeof(Attack), AIState.ATTACK}
    };

    protected virtual void Start()
    {
        _stateMachine = GetComponent<StateMachine>();
        if(!Player)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    protected virtual void FixedUpdate()
    {
        _AIThinkTimer -= Time.fixedDeltaTime;
        if(_AIThinkTimer <= 0.0f)
        {
            _AIThinkTimer = AIThinkRate;
            Think();
        }
    }

    protected virtual void Think()
    {
        //Debug.Log("Thinking about: " + GetCurrentAIState());
        switch(GetCurrentAIState())
        {
            case AIState.NONE:
                {
                    QueueState(GetNewPassiveState(true));
                    break;
                }
            case AIState.IDLE:
                {
                    if(LookForPlayer())
                    {
                        Alert(Player.transform.position, 4);
                    }
                    break;
                }
            case AIState.PATROL:
                {
                    if (LookForPlayer())
                    {
                        Alert(Player.transform.position, 3);
                    }
                    break;
                }
            case AIState.INVESTIGATE:
                {
                    if(LookForPlayer())
                    {
                        QueueState(new Attack(Player.transform, Lasers), true);
                    }
                    break;
                }
            case AIState.SEARCHNEARBY:
                {
                    if (LookForPlayer())
                    {
                        QueueState(new Attack(Player.transform, Lasers), true);
                    }
                    break;
                }
            case AIState.ATTACK:
                {
                    break;
                }
        }
    }

    public AIState GetCurrentAIState()
    {
        if(!_stateMachine)
        {
            return AIState.NONE;
        }

        if(_stateMachine.CurrentState == null)
        {
            return AIState.NONE;
        }

        return stateTypes[_stateMachine.CurrentState.GetType()];
    }

    public void Alert(Vector3 point, int priority)
    {
        QueueState(new Investigate(point, priority), true);
    }

    public void QueueState(State nextState, bool forceState = false)
    {
        //Debug.Log("Attempting to queue " + nextState + "\nForceEndPrevious: " + forceState);
        _stateMachine.CurrentState = nextState;
        if(forceState)
        {
            _stateMachine.ForceNextState();
        }
    }

    private State GetNewPassiveState(bool doAutoEnding)
    {
        float random = Random.value;

        if(random < 0.5f)
        {
            if(doAutoEnding)
            {
                return new Idle(Random.Range(1.0f, 7.0f));
            }
            else
            {
                return new Idle();
            }
        }
        else
        {
            if (doAutoEnding)
            {
                return new Patrol(PatrolPoints, Random.Range(5.0f, 60.0f), 10.0f);
            }
            else
            {
                return new Patrol(PatrolPoints, 10.0f);
            }
        }
    }

    private bool LookForPlayer()
    {
        Vector3 vectorToTarget = Player.transform.position - eyeTransform.position;
        float vectorToTargetMagnitude = vectorToTarget.magnitude;
        //Check to see if player is close enough to be seen
        if(vectorToTargetMagnitude > sightRange)
        {
            Debug.Log("Failed: too far away");
            return false;
        }

        //Check to see if player is within cone of vision
        if(Vector3.Angle(vectorToTarget, eyeTransform.forward) > fieldOfView)
        {
            Debug.Log("Failed: outside field of view");
            return false;
        }

        //Check to see if anything is blocking line of sight
        Ray lineOfSightRay = new Ray(eyeTransform.position, vectorToTarget);
        if (1 < Physics.RaycastNonAlloc(lineOfSightRay, new RaycastHit[2], vectorToTargetMagnitude, LineOfSightBlocking, QueryTriggerInteraction.Ignore))
        {
            Debug.Log("Failed: obstruction");
            return false;
        }

        //If every other check before now has failed, then the player can be seen.
        Debug.Log("Succeeded: player seen");
        return true;
    }
}
