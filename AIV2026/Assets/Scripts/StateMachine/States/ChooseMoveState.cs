using System.Runtime.CompilerServices;
using UnityEngine;

public class ChooseMoveState : BaseState
{
    public Jammer player1;
    public Jammer player2;

    public MoveCard attack;
    public MoveCard block;
    public MoveCard grapple;
    public MoveCard shove;
    public override void OnStateEnter()
    {
        base.OnStateEnter();
    }

    public override void OnStateStay()
    {
        HandlePlayer(player1);
        HandlePlayer(player2);

        if (player1.ChosenMove != null && player2.ChosenMove != null)
        {
            Resolve(player1, player2);
        }
    }
    private void HandlePlayer(Jammer jammer)
    {
        if (jammer.ChosenMove != null)
        {
            return;
        }

        if (jammer.PlayerType == PlayerType.Player1 || jammer.PlayerType == PlayerType.Player2)
        {
            HandleHumanInput(jammer);
        }
    }
    private void HandleHumanInput(Jammer jammer)
    {
        string c = jammer.Controller;

        if (Input.GetButtonDown(c + "_Attack")) jammer.ChosenMove = attack;
        if (Input.GetButtonDown(c + "_Block")) jammer.ChosenMove = block;
        if (Input.GetButtonDown(c + "_Grapple")) jammer.ChosenMove = grapple;
        if (Input.GetButtonDown(c + "_Shove")) jammer.ChosenMove = shove;
    }

    private void HandleAI(Jammer jammer)
    {
        MoveCard[] moves = { attack, block, grapple, shove };
        jammer.ChosenMove = moves[UnityEngine.Random.Range(0, moves.Length)];
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
    }
    
}
