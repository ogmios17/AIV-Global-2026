using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMashScript : MonoBehaviour
{

    private StateManager stateManager;
    private MiniMashState miniMashState;
    private PlayerBinder binder;
    private PlayerType playerType;
    private void onEnable()
    {
        stateManager = GlobalData.Instance.stateManager;
        Debug.Log(" state manager: ", stateManager);
        binder = GetComponent<PlayerBinder>();
        playerType = binder.Jammer.PlayerType;
        miniMashState = stateManager.MiniMashState;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Mash(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if(playerType == PlayerType.Player1)
        {
            miniMashState.Handler.Onp1Mash();
        }
        if (playerType == PlayerType.Player2)
        {
            miniMashState.Handler.Onp2Mash();
        }

    }
}
