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
    private HashSet<ITransition> anyTransition = new();

    public StateNode CurrentNode { get => current; }
    public StateNode PreviousNode { get => previous; }

    public StateNode NextNode { get => next; set => next = value; }

    public void Update()
    {
        var transition = GetTransition();
        if(transition != null)
        {
            ChangeState(transition.To);
        }

        current.State?.OnStateStay();
    }

    public void FixedUpdate()
    {
        current.State?.OnFixedStateStay();
    }

    public void SetState(IState state)
    {
        next = null;
        if(current != null)
        {
            previous = current;
        }
        current = nodes[state.GetType()];
        current.State?.OnStateEnter();
    }

    void ChangeState(IState state)
    {
        if (state == current.State) return;
        next = null;
        previous = current;
        var nextState = nodes[state.GetType()].State;
        previous.State?.OnStateExit();
        nextState?.OnStateEnter();
        current = nodes[state.GetType()];
    }

    ITransition GetTransition()
    {
        foreach(var transition in anyTransition) 
            if(transition.Condition.Evaluate())
                return transition;
        foreach(var transition in current.Transitions)
            if (transition.Condition.Evaluate())
                return transition;
        return null;
    }

    public void AddTransition(IState from, IState to, IPredicate condition)
    {
        GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
    }

    public void AddAnyTransition(IState to, IPredicate condition)
    {
        anyTransition.Add(new Transition(GetOrAddNode(to).State,condition));
    }

   public StateNode GetOrAddNode(IState state)
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
