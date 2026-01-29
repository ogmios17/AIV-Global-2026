using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public enum PlayerType
{
    Player1,
    Player2,
    CPU
}

public class ControllerHandler: MonoBehaviour
{
    [SerializeField] private PlayerInputManager playerInput;
    private bool bindingComplete;

    public bool BindingComplete => bindingComplete;

    public void OnPlayerJoined(PlayerInput input)
    {
        Debug.Log("Player joined: " + input.playerIndex);

        if (playerInput.playerCount >= 2)
        {
            bindingComplete = true;
        }
    }
   
}
