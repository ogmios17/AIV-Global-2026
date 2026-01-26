using UnityEngine;

public interface TransitionInterface
{
    StateInterface to { get; }
    Predicate condition { get; }
}
