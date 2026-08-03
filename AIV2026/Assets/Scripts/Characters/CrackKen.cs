/// <summary>
/// Crack-Ken: ability heals 1 on the next hit; ultimate fully heals on the next hit.
/// Both are armed by TriggerAbility/TriggerUlt and disarmed if the next move misses.
/// </summary>
public class CrackKen : CharacterBase
{
    public CrackKen(Jammer associatedPlayer)
    {
        this.associatedPlayer = associatedPlayer;
        associatedPlayer.onMoveMisses += AbilityMiss;
        associatedPlayer.onMoveMisses += UltiMiss;
        associatedPlayer.onMoveChosen += ResetAbilityTrigger;
        associatedPlayer.onMoveChosen += ResetUltiTrigger;
    }

    public override void Unsubscribe()
    {
        associatedPlayer.onMoveMisses -= AbilityMiss;
        associatedPlayer.onMoveMisses -= UltiMiss;
        associatedPlayer.onMoveChosen -= ResetAbilityTrigger;
        associatedPlayer.onMoveChosen -= ResetUltiTrigger;
        associatedPlayer.onMoveHits -= AbilityHits;
        associatedPlayer.onMoveHits -= UltiHits;
    }

    public override void TriggerAbility()
    {
        associatedPlayer.onMoveHits += AbilityHits;
        abilityTriggeredThisTurn = true;
    }

    public override void TriggerUlt()
    {
        associatedPlayer.onMoveHits += UltiHits;
        ultiTriggeredThisTurn = true;
    }

    private void UltiHits()
    {
        associatedPlayer.onMoveHits -= UltiHits;
        associatedPlayer.Cure(associatedPlayer.MaxHealth); // full heal; la UI segue via OnHealthChanged
    }

    private void AbilityHits()
    {
        associatedPlayer.onMoveHits -= AbilityHits;
        associatedPlayer.Cure();
    }

    private void UltiMiss()
    {
        associatedPlayer.onMoveHits -= UltiHits;
    }

    private void AbilityMiss()
    {
        associatedPlayer.onMoveHits -= AbilityHits;
    }
}
