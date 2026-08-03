/// <summary>
/// Base class for per-character ability/ultimate logic. Subclasses subscribe to the
/// owning Jammer's move events in their constructor and MUST detach them in Unsubscribe()
/// (called by Jammer when the character type changes, to avoid duplicate handlers).
/// </summary>
public abstract class CharacterBase
{
    protected Jammer associatedPlayer;
    protected bool abilityTriggeredThisTurn = false;
    protected bool ultiTriggeredThisTurn = false;

    public Jammer AssociatedPlayer { get => associatedPlayer; }
    public bool AbilityTriggeredThisTurn { get => abilityTriggeredThisTurn; }
    public bool UltiTriggeredThisTurn { get => ultiTriggeredThisTurn; }

    public virtual void TriggerAbility() { }
    public virtual void TriggerUlt() { }

    public virtual void ResetAbilityTrigger() { abilityTriggeredThisTurn = false; }
    public virtual void ResetUltiTrigger() { ultiTriggeredThisTurn = false; }

    public abstract void Unsubscribe();
}
