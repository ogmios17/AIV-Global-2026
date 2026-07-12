using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class NotZilla : ICharacter
{
    private int ultiCharge = 0;
    private GameObject popUp;
    public NotZilla(Jammer associatedPlayer)
    {
        this.CharacterData = GlobalData.Instance.Characters.Find(x=> x.characterType == associatedPlayer.CharacterType);
        this.associatedPlayer = associatedPlayer;
        associatedPlayer.onMoveMisses += AbilityMiss;
        associatedPlayer.onMoveMisses += ResetUltCounter;
        associatedPlayer.onMoveChosen += ResetAbilityTrigger;
        associatedPlayer.onMoveHits += ChargeUlti;

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
        if (GlobalData.Instance.Player1 == associatedPlayer)
        {
            FightersDataBinder fighter = GlobalData.Instance.Player2.CharacterPrefab.GetComponent<FightersDataBinder>();
            for(;ultiCharge>0; ultiCharge--)
                fighter.GetHit(GlobalData.Instance.Player2);
        }
        else
        {
            FightersDataBinder fighter = GlobalData.Instance.Player1.CharacterPrefab.GetComponent<FightersDataBinder>();
            for (; ultiCharge > 0; ultiCharge--)
                fighter.GetHit(GlobalData.Instance.Player1);
        }
        if (popUp == null) TryAssignPopUp();
        popUp.GetComponentInChildren<TextMeshProUGUI>().text = CharacterData.ultiText;
        popUp.SetActive(true);
    }


    public void AbilityHits()
    {
        Debug.Log("Ability hits");
        associatedPlayer.onMoveHits -= AbilityHits;
        if(GlobalData.Instance.Player1 == associatedPlayer)
            GlobalData.Instance.Player2.CharacterPrefab.GetComponent<FightersDataBinder>().GetHit(GlobalData.Instance.Player2);
        else
        {
            GlobalData.Instance.Player1.CharacterPrefab.GetComponent<FightersDataBinder>().GetHit(GlobalData.Instance.Player1);
        }
    }

    public override void ResetUltiTrigger()
    {
        ultiTriggeredThisTurn = false;
    }
    public void ResetUltCounter()
    {
        ultiCharge = 0;
    }

    public void AbilityMiss()
    {
        associatedPlayer.onMoveHits -= AbilityHits;
    }
    public override void ResetAbilityTrigger()
    {
        abilityTriggeredThisTurn = false;
    }

    public void ChargeUlti()
    {
        ultiCharge+=2;
        Debug.Log("Notzilla charges " + ultiCharge);
    }
}
