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
        associatedPlayer.onMoveMisses += UltiMiss;
        associatedPlayer.onMoveChosen += ResetAbilityTrigger;
        associatedPlayer.onMoveChosen += ResetUltiTrigger;
    }

    public void OnDestroy()
    {
        associatedPlayer.onMoveMisses -= AbilityMiss;
        associatedPlayer.onMoveMisses -= UltiMiss;
        associatedPlayer.onMoveChosen -= ResetAbilityTrigger;
        associatedPlayer.onMoveChosen -= ResetUltiTrigger;
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

    public void UltiHits()
    {
        associatedPlayer.onMoveHits -= UltiHits;
        associatedPlayer.CharacterPrefab.GetComponent<FightersDataBinder>().RefullLife(associatedPlayer);
    }

    public void AbilityHits()
    {
        associatedPlayer.onMoveHits -= AbilityHits;
        associatedPlayer.CharacterPrefab.GetComponent<FightersDataBinder>().Cure(associatedPlayer);
    }

    public void UltiMiss()
    {
        associatedPlayer.onMoveHits -= UltiHits;
    }
    public override void ResetUltiTrigger()
    {
        ultiTriggeredThisTurn = false;
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
