using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CrackKen : ICharacter
{
    private GameObject popUp;
    public CrackKen(Jammer associatedPlayer)
    {
        this.CharacterData = GlobalData.Instance.Characters.Find(x => x.characterType == associatedPlayer.CharacterType);
        this.associatedPlayer = associatedPlayer;
        associatedPlayer.onMoveMisses += AbilityMiss;
        associatedPlayer.onMoveMisses += UltiMiss;
        associatedPlayer.onMoveChosen += ResetAbilityTrigger;
        associatedPlayer.onMoveChosen += ResetUltiTrigger;

        TryAssignPopUp();
    }

    public void TryAssignPopUp()
    {
        switch (associatedPlayer.PlayerType)
        {
            case PlayerType.Player1:
                popUp = DataFightHelper.Instance?.P1popUp;
                break;
            case PlayerType.Player2:
            case PlayerType.CPU:
                popUp = DataFightHelper.Instance?.P2popUp;
                break;

        }
    }

    public override void TriggerAbility()
    {
        associatedPlayer.onMoveHits += AbilityHits;
        abilityTriggeredThisTurn = true;
        if (popUp == null) TryAssignPopUp();
        popUp.GetComponentInChildren<TextMeshProUGUI>().text = CharacterData.abilityText;
        popUp.SetActive(true);
    }

    public override void TriggerUlt()
    {
        associatedPlayer.onMoveHits += UltiHits;
        ultiTriggeredThisTurn = true;
        if (popUp == null) TryAssignPopUp();
        popUp.GetComponentInChildren<TextMeshProUGUI>().text = CharacterData.ultiText;
        popUp.SetActive(true);
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
