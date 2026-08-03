/// <summary>
/// NotZilla: every hit charges the ultimate; the ability deals 1 extra damage on the
/// next hit; the ultimate unloads all accumulated charges as damage on the opponent.
/// </summary>
public class NotZilla : CharacterBase
{
    private int ultiCharge = 0;

    public NotZilla(Jammer associatedPlayer)
    {
        this.associatedPlayer = associatedPlayer;
        associatedPlayer.onMoveMisses += AbilityMiss;
        associatedPlayer.onMoveChosen += ResetAbilityTrigger;
        associatedPlayer.onMoveHits += ChargeUlti;
    }

    public override void Unsubscribe()
    {
        associatedPlayer.onMoveMisses -= AbilityMiss;
        associatedPlayer.onMoveChosen -= ResetAbilityTrigger;
        associatedPlayer.onMoveHits -= ChargeUlti;
        associatedPlayer.onMoveHits -= AbilityHits;
    }

    public override void TriggerAbility()
    {
        associatedPlayer.onMoveHits += AbilityHits;
        abilityTriggeredThisTurn = true;
    }

    public override void TriggerUlt()
    {
        if (ultiCharge <= 0) return;

        GlobalData.Instance.GetOpponent(associatedPlayer).TakeAHit(ultiCharge);
        ultiCharge = 0;
    }

    private void AbilityHits()
    {
        associatedPlayer.onMoveHits -= AbilityHits;
        GlobalData.Instance.GetOpponent(associatedPlayer).TakeAHit();
    }

    private void AbilityMiss()
    {
        associatedPlayer.onMoveHits -= AbilityHits;
    }

    private void ChargeUlti()
    {
        ultiCharge++;
    }
}
