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

        // Se sei il Player 1 e non hai ancora scelto una carta triggera l'animazione
        if (binder.Jammer.PlayerType == PlayerType.Player1 && GlobalData.Instance.Player1.ChosenMove == null)
        {
            binder.Jammer.FighterAnim.SetTrigger("Attack");
            binder.Jammer.CardsAnim.SetTrigger("Attack");
        }

        // Se sei il Player 2 e non hai ancora scelto una carta triggera l'animazione
        if (binder.Jammer.PlayerType == PlayerType.Player2 && GlobalData.Instance.Player2.ChosenMove == null)
        {
            binder.Jammer.FighterAnim.SetTrigger("Attack");
            binder.Jammer.CardsAnim.SetTrigger("Attack");
        }

        // Se il giocatore ha già scelto una carta esco (non devo giocare ancora)
        if (binder.Jammer.ChosenMove != null) return;

        // Salvo la scelta della carta
        if (binder.Jammer.PlayerType == PlayerType.Player1)
            moveState.OnP1Received(attack);
        else
            moveState.OnP2Received(attack);
        
        // Debug.Log("Attack");
    }

    public void Block(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        // Se sei il Player 1 e non hai ancora scelto una carta triggera l'animazione
        if (binder.Jammer.PlayerType == PlayerType.Player1 && GlobalData.Instance.Player1.ChosenMove == null)
        {
            binder.Jammer.FighterAnim.SetTrigger("Block");
            binder.Jammer.CardsAnim.SetTrigger("Block");
        }

        // Se sei il Player 2 e non hai ancora scelto una carta triggera l'animazione
        if (binder.Jammer.PlayerType == PlayerType.Player2 && GlobalData.Instance.Player2.ChosenMove == null)
        {
            binder.Jammer.FighterAnim.SetTrigger("Block");
            binder.Jammer.CardsAnim.SetTrigger("Block");
        }

        // Se il giocatore ha già scelto una carta esco (non devo giocare ancora)
        if (binder.Jammer.ChosenMove != null) return;

        // Salvo la scelta della carta
        if (binder.Jammer.PlayerType == PlayerType.Player1)
            moveState.OnP1Received(block);
        else
            moveState.OnP2Received(block);
        // Debug.Log("Block");
    }

    public void Grapple(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        // Se sei il Player 1 e non hai ancora scelto una carta triggera l'animazione
        if (binder.Jammer.PlayerType == PlayerType.Player1 && GlobalData.Instance.Player1.ChosenMove == null)
        {
            binder.Jammer.FighterAnim.SetTrigger("Grapple");
            binder.Jammer.CardsAnim.SetTrigger("Grapple");
        }

        // Se sei il Player 2 e non hai ancora scelto una carta triggera l'animazione
        if (binder.Jammer.PlayerType == PlayerType.Player2 && GlobalData.Instance.Player2.ChosenMove == null)
        {
            binder.Jammer.FighterAnim.SetTrigger("Grapple");
            binder.Jammer.CardsAnim.SetTrigger("Grapple");
        }

        // Se il giocatore ha già scelto una carta esco (non devo giocare ancora)
        if (binder.Jammer.ChosenMove != null) return;

        // Salvo la scelta della carta
        if (binder.Jammer.PlayerType == PlayerType.Player1)
            moveState.OnP1Received(grapple);
        else
            moveState.OnP2Received(grapple);
        // Debug.Log("Grapple");
    }

    public void Shove(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        // Se sei il Player 1 e non hai ancora scelto una carta triggera l'animazione
        if (binder.Jammer.PlayerType == PlayerType.Player1 && GlobalData.Instance.Player1.ChosenMove == null)
        {
            binder.Jammer.FighterAnim.SetTrigger("Shove");
            binder.Jammer.CardsAnim.SetTrigger("Shove");
        }

        // Se sei il Player 2 e non hai ancora scelto una carta triggera l'animazione
        if (binder.Jammer.PlayerType == PlayerType.Player2 && GlobalData.Instance.Player2.ChosenMove == null)
        {
            binder.Jammer.FighterAnim.SetTrigger("Shove");
            binder.Jammer.CardsAnim.SetTrigger("Shove");
        }

        // Se il giocatore ha già scelto una carta esco (non devo giocare ancora)
        if (binder.Jammer.ChosenMove != null) return;

        // Salvo la scelta della carta
        if (binder.Jammer.PlayerType == PlayerType.Player1)
            moveState.OnP1Received(shove);
        else
            moveState.OnP2Received(shove);
        // Debug.Log("Shove");
    }
}
