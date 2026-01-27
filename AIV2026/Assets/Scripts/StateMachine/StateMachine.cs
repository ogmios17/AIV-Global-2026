using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class StateMachine
{
    private StateNode current;
    private StateNode previous;
    private StateNode next;
    private Dictionary<Type, StateNode> nodes = new();
    private HashSet<TransitionInterface> anyTransition = new();

    public StateNode CurrentNode { get => current; }
    public StateNode PreviousNode { get => previous; }

    public StateNode NextNode { get => next; set => next = value; }

    public void Update()
    {
        var transition = GetTransition();
        if(transition != null)
        {
            ChangeState(transition.to);
        }

        current.state?.OnStateStay();
    }

    public void FixedUpdate()
    {
        current.state?.OnFixedStateStay();
    }

    public void SetState(StateInterface state)
    {
        next = null;
        if(current != null)
        {
            previous = current;
        }
        current = nodes[state.GetType()];
        current.state?.OnStateEnter();
    }

    void ChangeState(StateInterface state)
    {
        if (state == current.state) return;
        next = null;
        previous = current;
        var nextState = nodes[state.GetType()].state;
        previous.state?.OnStateExit();
        nextState?.OnStateEnter();
        current = nodes[state.GetType()];
    }

    TransitionInterface GetTransition()
    {
        foreach(var transition in anyTransition) 
            if(transition.condition.Evaluate())
                return transition;
        foreach(var transition in current.transitions)
            if (transition.condition.Evaluate())
                return transition;
        return null;
    }

    public void AddTransition(StateInterface from, StateInterface to, Predicate condition)
    {
        GetOrAddNode(from).AddTransition(GetOrAddNode(to).state, condition);
    }

    public void AddAnyTransition(StateInterface to, Predicate condition)
    {
        anyTransition.Add(new Transition(GetOrAddNode(to).state,condition));
    }

   public StateNode GetOrAddNode(StateInterface state)
    {
        var node = nodes.GetValueOrDefault(state.GetType());  
        
        if(node == null)
        {
            node = new StateNode(state);
            nodes.Add(state.GetType(), node);
        }

        return node;
    }
}
