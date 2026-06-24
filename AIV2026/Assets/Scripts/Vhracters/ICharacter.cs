using UnityEngine;

public abstract class ICharacter
{
    protected Jammer associatedPlayer;
    protected bool abilityTriggeredThisTurn = false;

    public Jammer AssociatedPlayer { get => associatedPlayer; set => associatedPlayer = value; }
    public bool AbilityTriggeredThisTurn { get => abilityTriggeredThisTurn; set => abilityTriggeredThisTurn = value; }

    public virtual void TriggerAbility() { }
    public virtual void TriggerUlt() { }

    public virtual void ResetAbilityTrigger()
    {
    }

}
