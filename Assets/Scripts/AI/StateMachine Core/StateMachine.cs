using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public State CurrentState
    {
        get { return _activeState; }
        set
        {
            _queuedNextState = value;
        }
    }
    protected State _queuedNextState;
    protected State _activeState;
    protected State.StatePhase _oldStatePhase = State.StatePhase.INACTIVE;

    public GameObject ReleventGameObject;

    protected virtual void FixedUpdate()
    {
        StateMachineProcessState();
    }

    protected virtual void StateMachineProcessState()
    {
        //Figure out what GameObject should be passed into state
        GameObject gameObjectToProccess = gameObject;
        if (ReleventGameObject)
        {
            gameObjectToProccess = ReleventGameObject;
        }

        //Start the queued state if active state is done.
        if (_activeState == null && _queuedNextState != null)
        {
            _activeState = _queuedNextState;
        }

        //Run state behavior for _activeState
        if(_activeState != null)
        {
            State.StatePhase phaseOnStateProcessing = _activeState.CurrentPhase;

            switch (_activeState.CurrentPhase)
            {
                case State.StatePhase.ENTERING:
                    {
                        if (_oldStatePhase != _activeState.CurrentPhase)
                        {
                            _activeState.OnEnter(gameObjectToProccess);
                        }
                        else
                        {
                            _activeState.EnterBehavior(gameObjectToProccess);
                        }
                        break;
                    }
                case State.StatePhase.ACTIVE:
                    {
                        _activeState.ActiveBehavior(gameObjectToProccess);
                        break;
                    }
                case State.StatePhase.EXITING:
                    {
                        if (_oldStatePhase != _activeState.CurrentPhase)
                        {
                            _activeState.OnExit(gameObjectToProccess);
                        }
                        else
                        {
                            _activeState.ExitBehavior(gameObjectToProccess);
                        }
                        break;
                    }
                case State.StatePhase.INACTIVE:
                    {
                        _activeState = null;
                        break;
                    }
            }

            _oldStatePhase = phaseOnStateProcessing;
        }
    }

    public void ForceNextState()
    {
        if(_activeState != null)
        {
            _activeState.ForceEndState();
        }
    }
}
