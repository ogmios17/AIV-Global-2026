using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
public enum CardTypes
{
    Attack,
    Block,
    Shove,
    Grapple
}

[CreateAssetMenu(fileName = "ChooseMoveState", menuName = "Scriptable Objects/ChooseMoveState")]
public class ChooseMoveState : ScriptableObject, StateInterface
{
    public Jammer player1;
    public Jammer player2;
    public bool goToMinigame = false;
    public GameObject prefab;
    private GameObject prefabClone;


    public void OnStateEnter()
    {
        player1 = GlobalData.Instance.Player1;
        player2 = GlobalData.Instance.Player2;

        Debug.Log(GlobalData.Instance.Player1);
        Debug.Log(GlobalData.Instance.Player2);
        goToMinigame = false;

        prefabClone = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        player1.Input.gameObject.GetComponent<PlayerMoveInput>().enabled = true;
        player2.Input.gameObject.GetComponent<PlayerMoveInput>().enabled = true;

        player1.Input.SwitchCurrentActionMap("CardSelection");
        player2.Input.SwitchCurrentActionMap("CardSelection");

    }

    public void OnStateStay()
    {

        if (player1.ChosenMove != null && player2.ChosenMove != null)
        {
            Resolve(player1, player2);
        }
    }
  
    private void Resolve(Jammer p1, Jammer p2)
    {
        MoveCard c1 = p1.ChosenMove;
        MoveCard c2 = p2.ChosenMove;

        if (c1 == c2 || c1.draws.Contains(c2))
        {
            Debug.Log("DRAW");
            AudioManager.Instance.PlayCancelCard();
        }
        else if (c1.clashes.Contains(c2))
        {
            Debug.Log("Clash");
            AudioManager.Instance.PlayCancelCard();
        }
        else if (c1.wins == c2)
        {
            player2.CharacterPrefab.GetComponent<FightersDataBinder>().GetHit(player2);
            PlayerCardSound(p1);
            Debug.Log("PLAYER 1 WINS");
        }
        else if (c1.loses == c2)
        {
            player1.CharacterPrefab.GetComponent<FightersDataBinder>().GetHit(player1);
            PlayerCardSound(p2);
            Debug.Log("PLAYER 1 LOSE");
        }

        p2.ChosenMove = null;
        p1.ChosenMove = null;

        //goToMinigame = true; //ELIMINA DOPO IL DEBUG 


    }

    private void PlayerCardSound(Jammer player)
    {
        var input = player.Input.GetComponent<PlayerMoveInput>();
        MoveCard chosen = player.ChosenMove;

        if (chosen == input.AttackCard)
        {
            AudioManager.Instance.PlayAttackCard();
        }
        else if (chosen == input.BlockCard)
        {
            AudioManager.Instance.PlayBlockCard();

        }
        else if (chosen == input.GrappleCard)
        {
            AudioManager.Instance.PlayGrabCard();
        }
        else if (chosen == input.ShoveCard)
        {
            AudioManager.Instance.PlayPushCard();
        }
    }
    public void OnFixedStateStay()
    {

    }

    public void OnStateExit()
    {
        player1.Input.gameObject.GetComponent<PlayerMoveInput>().enabled = false;
        player2.Input.gameObject.GetComponent<PlayerMoveInput>().enabled = false;
    }
    
}
