using System.Runtime.CompilerServices;
using Unity.VisualScripting;
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

    public void OnStateEnter()
    {
        player1 = GlobalData.Instance.Player1;
        Debug.Log("player1 " + player1.PlayerType);
        player2 = GlobalData.Instance.Player2;

        Debug.Log(GlobalData.Instance.Player1);
        Debug.Log(GlobalData.Instance.Player2);
        goToMinigame = false;

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
        }
        else if (c1.wins == c2)
        {
            Debug.Log("PLAYER 1 WINS");
        }
        else if (c1.loses == c2)
        {
            Debug.Log("PLAYER 1 LOSE");
        }

        p2.ChosenMove = null;
        p1.ChosenMove = null;

        goToMinigame = true; //ELIMINA DOPO IL DEBUG 


    }

    public void OnFixedStateStay()
    {

    }

    public void OnStateExit()
    {

    }
    
}
