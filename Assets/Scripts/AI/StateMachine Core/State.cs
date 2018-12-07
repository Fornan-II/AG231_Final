using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public enum StatePhase
    {
        ENTERING,
        ACTIVE,
        EXITING,
        INACTIVE
    }

    protected StatePhase _currentPhase = StatePhase.ENTERING;
    public StatePhase CurrentPhase { get { return _currentPhase; } }

    public abstract void OnEnter(GameObject gameObject);

    public abstract void ActiveBehavior(GameObject gameObject);

    public abstract void OnExit(GameObject gameObject);

    public virtual void EnterBehavior(GameObject gameObject) { }

    public virtual void ExitBehavior(GameObject gameObject) { }

    public virtual void ForceEndState()
    {
        _currentPhase = StatePhase.EXITING;
    }
}
