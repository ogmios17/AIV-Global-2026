using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMashScript : MonoBehaviour
{

    private StateManager stateManager;
    private MiniMashState miniMashState;
    private PlayerBinder binder;
    private PlayerType playerType;

    private void OnEnable()
    {
        stateManager = GlobalData.Instance.stateManager;
        binder = GetComponent<PlayerBinder>();
        playerType = binder.Jammer.PlayerType;
        miniMashState = stateManager.MiniMashState;
    }

    public void Mash(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if(playerType == PlayerType.Player1)
        {
            miniMashState.Handler.Onp1Mash();
            AudioManager.Instance.PlaySpamButtonP1();
        }
        if (playerType == PlayerType.Player2)
        {
            miniMashState.Handler.Onp2Mash();
            AudioManager.Instance.PlaySpamButtonP2();
        }

    }
}
