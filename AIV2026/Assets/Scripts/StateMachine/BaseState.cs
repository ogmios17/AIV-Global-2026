using UnityEngine;

public class BaseState:  StateInterface
{
    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }
    public virtual void OnStateStay() { }
    public virtual void OnFixedStateStay() { }
}

