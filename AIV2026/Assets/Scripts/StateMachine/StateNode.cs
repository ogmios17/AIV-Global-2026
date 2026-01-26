using System.Collections.Generic;
using UnityEngine;

public class StateNode
{
    public StateInterface state { get; }
    public HashSet<TransitionInterface> transitions { get; }

    public StateNode(StateInterface state)
    {
        this.state = state;
        transitions = new HashSet<TransitionInterface>();
    }

    public void AddTransition(StateInterface to, Predicate condition)
    {
        transitions.Add(new Transition(to,condition));
    }
}
