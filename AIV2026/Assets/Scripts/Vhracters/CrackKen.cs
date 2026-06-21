using UnityEngine;

public class CrackKen : ICharacter
{
    public CrackKen(Jammer associatedPlayer)
    {
        this.associatedPlayer = associatedPlayer;
    }

    public void Awake()
    {
        associatedPlayer.onMoveMisses += AbilityMiss;
    }

    public void OnDestroy()
    {
        associatedPlayer.onMoveMisses -= AbilityMiss;
    }

    public override void TriggerAbility()
    {
        associatedPlayer.onMoveHits += AbilityHits; 
    }

    public override void TriggerUlt()
    {

    }

    public void AbilityHits()
    {
        associatedPlayer.onMoveHits -= AbilityHits;
        associatedPlayer.Cure();
    }

    public void AbilityMiss()
    {
        associatedPlayer.onMoveHits -= AbilityHits;
    }
}
