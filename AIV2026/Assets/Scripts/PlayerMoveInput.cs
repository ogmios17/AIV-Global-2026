using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerMoveInput : MonoBehaviour
{
    private PlayerBinder binder;

    [SerializeField] private MoveCard attack;
    [SerializeField] private MoveCard block;
    [SerializeField] private MoveCard grapple;
    [SerializeField] private MoveCard shove;

    public MoveCard AttackCard => attack;
    public MoveCard BlockCard => block;
    public MoveCard GrappleCard => grapple;
    public MoveCard ShoveCard => shove;

    private void Awake()
    {
        binder = GetComponent<PlayerBinder>();
    }

    public void Attack(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        binder.Jammer.ChosenMove = attack;
        binder.Jammer.FighterAnim.SetTrigger("Attack");
        Debug.Log("Attack");
    }

    public void Block(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        binder.Jammer.ChosenMove = block;
        binder.Jammer.FighterAnim.SetTrigger("Block");
        Debug.Log("Block");
    }

    public void Grapple(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        binder.Jammer.ChosenMove = grapple;
        binder.Jammer.FighterAnim.SetTrigger("Grapple");
        Debug.Log("Grapple");
    }

    public void Shove(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        binder.Jammer.ChosenMove = shove;
        binder.Jammer.FighterAnim.SetTrigger("Shove");
        Debug.Log("Shove");
    }

    
}
