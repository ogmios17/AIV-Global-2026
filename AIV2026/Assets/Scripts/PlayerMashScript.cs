using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMashScript : MonoBehaviour
{
    public float requestedMashes = 50;

    private StateManager stateManager;
    private MiniMashState miniMashState;
    private PlayerBinder binder;
    private PlayerType playerType;
    private void Awake()
    {
        stateManager = GlobalData.Instance.stateManager;
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
            miniMashState.Onp1Mash();
        }
        if (playerType == PlayerType.Player2)
        {
            miniMashState.Onp2Mash();
        }

    }
}
