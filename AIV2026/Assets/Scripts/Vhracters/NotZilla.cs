using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class NotZilla : ICharacter
{
    private int ultiCharge = 0;
    public NotZilla(Jammer associatedPlayer)
    {
        this.associatedPlayer = associatedPlayer;
        associatedPlayer.onMoveMisses += AbilityMiss;
        associatedPlayer.onMoveChosen += ResetAbilityTrigger;
        associatedPlayer.onMoveHits += ChargeUlti;
    }

    public override void TriggerAbility()
    {
        associatedPlayer.onMoveHits += AbilityHits;
        abilityTriggeredThisTurn = true;
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
        ultiCharge++;
        Debug.Log("Notzilla charges " + ultiCharge);
    }
}
