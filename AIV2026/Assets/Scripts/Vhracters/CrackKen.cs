using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

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
        abilityTriggeredThisTurn = true;
    }

    public override void TriggerUlt()
    {

    }

    public void AbilityHits()
    {
        associatedPlayer.onMoveHits -= AbilityHits;
        associatedPlayer.CharacterPrefab.GetComponent<FightersDataBinder>().Cure(associatedPlayer);
    }

    public void AbilityMiss()
    {
        associatedPlayer.onMoveHits -= AbilityHits;
    }
    public override void ResetAbilityTrigger()
    {
        abilityTriggeredThisTurn = false;
    }
}
