using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSequenceInput : MonoBehaviour
{
    private StateManager stateManager;
    private MiniSequenceState miniSequenceState;
    private PlayerBinder binder;
    private PlayerType playerType;
    private void Awake()
    {
        stateManager = GlobalData.Instance.stateManager;
        Debug.Log(" state manager: ", stateManager);
        binder = GetComponent<PlayerBinder>();
        playerType = binder.Jammer.PlayerType;
        miniSequenceState = stateManager.MiniSequenceState;
    }
    void Start()
    {

    }

    void Update()
    {

    }

    public void Press(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        string pressed = ctx.control.path;
        // Debug.Log("pressed " + pressed);
        if (playerType == PlayerType.Player1)
        {
            miniSequenceState.Handler.Onp1Press(pressed);
        }
        if (playerType == PlayerType.Player2)
        {
            miniSequenceState.Handler.Onp2Press(pressed);
        }

    }
}
