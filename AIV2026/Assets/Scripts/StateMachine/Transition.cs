using UnityEngine;

public class Transition:TransitionInterface
{
    public StateInterface to { get; }
    public Predicate condition { get; }

    public Transition(StateInterface to, Predicate condition)
    {
        this.to = to;
        this.condition = condition;
    }
}
