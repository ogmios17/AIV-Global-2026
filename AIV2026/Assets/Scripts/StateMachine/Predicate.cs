using System;
using UnityEngine;

public interface Predicate
{
    bool Evaluate();
}

public class FuncPredicate : Predicate
{
    readonly Func<bool> func;

    public FuncPredicate(Func<bool> func)
    {
        this.func = func;
    }

    public bool Evaluate()
    {
        return func.Invoke();
    }
}