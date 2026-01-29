using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveInput : MonoBehaviour
{
    private PlayerBinder binder;

    [SerializeField] private MoveCard attack;
    [SerializeField] private MoveCard block;
    [SerializeField] private MoveCard grapple;
    [SerializeField] private MoveCard shove;

    private void Awake()
    {
        binder = GetComponent<PlayerBinder>();
    }

    public void Attack(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        binder.Jammer.ChosenMove = attack;
    }

    public void Block(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        binder.Jammer.ChosenMove = block;
    }

    public void Grapple(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        binder.Jammer.ChosenMove = grapple;
    }

    public void Shove(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        binder.Jammer.ChosenMove = shove;
    }

    
}
