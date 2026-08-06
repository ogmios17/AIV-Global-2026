using UnityEngine;

public abstract class ICharacter
{
    protected Jammer associatedPlayer;
    protected Jammer enemyPlayer;
    protected bool abilityTriggeredThisTurn = false;
    protected bool ultiTriggeredThisTurn = false;
    private CharacterSO characterData;

    public Jammer AssociatedPlayer { get => associatedPlayer; set => associatedPlayer = value; }
    public bool AbilityTriggeredThisTurn { get => abilityTriggeredThisTurn; set => abilityTriggeredThisTurn = value; }
    public bool UltiTriggeredThisTurn { get => ultiTriggeredThisTurn; set => ultiTriggeredThisTurn = value; }
    public Jammer EnemyPlayer { get => enemyPlayer; set => enemyPlayer = value; }
    public CharacterSO CharacterData { get => characterData; set => characterData = value; }

    public virtual void TriggerAbility() { }
    public virtual void TriggerUlt() { }

    public virtual void ResetAbilityTrigger()
    {
    }

    public virtual void ResetUltiTrigger() { }

}
