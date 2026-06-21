using UnityEngine;

public abstract class ICharacter
{
    protected Jammer associatedPlayer;

    public Jammer AssociatedPlayer { get => associatedPlayer; set => associatedPlayer = value; }

    public virtual void TriggerAbility() { }
    public virtual void TriggerUlt() { }

}
