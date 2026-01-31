using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUIInput : MonoBehaviour
{

    private StateManager stateManager;
    private IdleState idleState;
    private PlayerBinder binder;
    private PlayerType playerType;

    void Awake()
    {
        stateManager = GlobalData.Instance.stateManager;
        idleState = stateManager.IdleState;
        binder = GetComponent<PlayerBinder>();
        playerType = binder.Jammer.PlayerType;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSubmitPressed(InputAction.CallbackContext ctx)
    {
        idleState.Handler.OnSubmitPressed(playerType);
    }

    public void Navigate(InputAction.CallbackContext ctx)
    {
        if (playerType == PlayerType.Player1)
        {
            idleState.Handler.HandleNavigationP1(ctx.ReadValue<Vector2>());
        }
        else if (playerType == PlayerType.Player2)
        {
            idleState.Handler.HandleNavigationP2(ctx.ReadValue<Vector2>());
        }
    }
}
