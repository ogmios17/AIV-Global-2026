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

    [Header("Available Move Cards")]
    public MoveCard[] availableMoves;

    public void OnStateEnter()
    {
        player1 = GlobalData.Instance.Player1;
        player2 = GlobalData.Instance.Player2;

        Debug.Log(GlobalData.Instance.Player1);
        Debug.Log(GlobalData.Instance.Player2);
        goToMinigame = false;

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
            // int randomIndex = Random.Range(0, 4);
            // player2.ChosenMove = availableMoves[randomIndex];
            player2.ChosenMove = availableMoves[0]; // Attack
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

        if (c1 == c2 || c1.draws.Contains(c2))
        {
            Debug.Log("DRAW");
        }
        else if (c1.clashes.Contains(c2))
        {
            Debug.Log("Clash");
        }
        else if (c1.wins == c2)
        {
            player2.CharacterPrefab.GetComponent<FightersDataBinder>().GetHit(player2);
            Debug.Log("PLAYER 1 WINS");
        }
        else if (c1.loses == c2)
        {
            player1.CharacterPrefab.GetComponent<FightersDataBinder>().GetHit(player1);
            Debug.Log("PLAYER 1 LOSE");
        }

        p2.ChosenMove = null;
        p1.ChosenMove = null;

        //goToMinigame = true; //ELIMINA DOPO IL DEBUG 


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
    
}
