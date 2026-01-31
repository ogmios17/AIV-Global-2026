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
    public GameObject prefab;
    private GameObject prefabClone;
    private int nextMinigame;

    [Header("Available Move Cards")]
    public MoveCard[] availableMoves;

    public int NextMinigame { get => nextMinigame; }

    public void OnStateEnter()
    {
        nextMinigame = -1;

        player1 = GlobalData.Instance.Player1;
        player2 = GlobalData.Instance.Player2;

        Debug.Log(GlobalData.Instance.Player1);
        Debug.Log(GlobalData.Instance.Player2);

        prefabClone = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);

        // Human P1
        player1.Input.gameObject.GetComponent<PlayerMoveInput>().enabled = true;
        player1.Input.SwitchCurrentActionMap("CardSelection");

        // Human P2
        if (!player2.IsCPUMode)
        {
            player2.Input.gameObject.GetComponent<PlayerMoveInput>().enabled = true;
            player2.Input.SwitchCurrentActionMap("CardSelection");
        }
    }

    public void OnStateStay()
    {   
        // CPU sceglie mossa random se non l'ha già fatto
        if (player2.IsCPUMode && player2.ChosenMove == null)
        {
            int randomIndex = Random.Range(0, 4);
            player2.ChosenMove = availableMoves[randomIndex];
            
            // player2.ChosenMove = availableMoves[0]; // DEBUG: Attack
            Debug.Log($"CPU sceglie: {player2.ChosenMove.cardName}");
        }

        if (player1.ChosenMove != null && player2.ChosenMove != null)
        {
            Resolve(player1, player2);
        }
    }
  
    private void Resolve(Jammer p1, Jammer p2)
    {
        MoveCard c1 = p1.ChosenMove;
        MoveCard c2 = p2.ChosenMove;

        if (c1.draws.Contains(c2))
        {
            player2.FighterAnim.SetTrigger("Next");
            player1.FighterAnim.SetTrigger("Next");
            Debug.Log("DRAW");
            AudioManager.Instance.PlayCancelCard();
        }
        else if (c1.clashes.Contains(c2))
        {
            player2.FighterAnim.SetTrigger("Next");
            player1.FighterAnim.SetTrigger("Next");
            ChooseMinigame();
            Debug.Log("Clash");
            AudioManager.Instance.PlayCancelCard();
        }
        else if (c1.wins == c2)
        {
            player2.FighterAnim.SetTrigger("Damage");
            player2.CharacterPrefab.GetComponent<FightersDataBinder>().GetHit(player2);
            PlayerCardSound(p1);
            Debug.Log("PLAYER 1 WINS");
        }
        else if (c1.loses == c2)
        {
            player1.FighterAnim.SetTrigger("Damage");
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
        if (!player2.IsCPUMode)
            player2.Input.gameObject.GetComponent<PlayerMoveInput>().enabled = false;
    }
    
    public void ChooseMinigame()
    {
        int index = Random.Range(0, 1);
        nextMinigame = index;
    }
}
