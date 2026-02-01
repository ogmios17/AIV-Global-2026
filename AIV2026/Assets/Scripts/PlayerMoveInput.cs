using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerMoveInput : MonoBehaviour
{
    private PlayerBinder binder;
    private ChooseMoveState moveState;

    [SerializeField] private MoveCard attack;
    [SerializeField] private MoveCard block;
    [SerializeField] private MoveCard grapple;
    [SerializeField] private MoveCard shove;


    private void Awake()
    {
        binder = GetComponent<PlayerBinder>();
    }

    private void Start()
    {
        moveState = GlobalData.Instance.stateManager.ChooseMoveState;
    }

    private void Update()
    {
        
    }

    public void Attack(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if (binder.Jammer.ChosenMove != null) return;
        if (binder.Jammer.PlayerType == PlayerType.Player1)
        {
            if (moveState.P1Card == null)
            {
                binder.Jammer.FighterAnim.SetTrigger("Attack");
                binder.Jammer.CardsAnim.SetTrigger("Attack");
            }
            moveState.OnP1Received(shove);
        }
        else
        {
            if (moveState.P2Card == null)
            {
                binder.Jammer.FighterAnim.SetTrigger("Attack");
                binder.Jammer.CardsAnim.SetTrigger("Attack");
            }
            moveState.OnP2Received(shove);
            Debug.Log("Attack");
        }
    }

    public void Block(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if (binder.Jammer.ChosenMove != null) return;
        if (binder.Jammer.PlayerType == PlayerType.Player1)
        {
            if (moveState.P1Card == null)
            {
                binder.Jammer.FighterAnim.SetTrigger("Block");
                binder.Jammer.CardsAnim.SetTrigger("Block");
            }
            moveState.OnP1Received(shove);
        }
        else
        {
            if (moveState.P2Card == null)
            {
                binder.Jammer.FighterAnim.SetTrigger("Block");
                binder.Jammer.CardsAnim.SetTrigger("Block");
            }
            moveState.OnP2Received(shove);
            Debug.Log("Block");
        }
    }

    public void Grapple(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if (binder.Jammer.ChosenMove != null) return;
        if (binder.Jammer.PlayerType == PlayerType.Player1)
        {
            if (moveState.P1Card == null)
            {
                binder.Jammer.FighterAnim.SetTrigger("Grapple");
                binder.Jammer.CardsAnim.SetTrigger("Grapple");
            }
            moveState.OnP1Received(shove);
        }
        else
        {
            if (moveState.P2Card == null)
            {
                binder.Jammer.FighterAnim.SetTrigger("Grapple");
                binder.Jammer.CardsAnim.SetTrigger("Grapple");
            }
            moveState.OnP2Received(shove);
        }
    }

    public void Shove(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if (binder.Jammer.ChosenMove != null) return;
        if (binder.Jammer.PlayerType == PlayerType.Player1)
        {
            if (moveState.P1Card == null)
            {
                binder.Jammer.FighterAnim.SetTrigger("Shove");
                binder.Jammer.CardsAnim.SetTrigger("Shove");
            }
            moveState.OnP1Received(shove);
        }
        else
        {
            if (moveState.P2Card == null)
            {
                binder.Jammer.FighterAnim.SetTrigger("Shove");
                binder.Jammer.CardsAnim.SetTrigger("Shove");
            }
            moveState.OnP2Received(shove);
        }
        Debug.Log("Shove");
    }

    
}
