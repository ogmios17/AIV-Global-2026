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
    string[] joystickNames;
    private Jammer player1;
    private Jammer player2;
    private bool bindingComplete;
    private bool resetConnection;
    private int numOfPlayers = 0;
    [SerializeField]
    private PlayerInputManager playerInput;
    public bool BindingComplete { get => bindingComplete; }
    public bool ResetConnection { get => resetConnection; }
    public Jammer Player1 { get => player1; }
    public Jammer Player2 { get => player2; }

    public PlayerInputManager PlayerInput { set => playerInput = value; }

    //public void StartLookingForControllers()
    //{
    //    resetConnection = false;
    //    bindingComplete = false;
    //    GetConnectedControllers();
    //}

    //public void GetConnectedControllers()
    //{
    //    joystickNames = Input.GetJoystickNames();
    //    numOfPlayers = joystickNames.Length;
    //    bindingComplete = true;
    //    StartCoroutine(CheckControllers());


    //}

    private void Start()
    {
        player1 = new Jammer();
        player2 = new Jammer();
    }

    public void CheckNumOfPlayers()
    {
        Debug.Log("Player count: " + playerInput.playerCount);
        if (playerInput.playerCount > 1)
        {
            bindingComplete = true;
        }
    }
    public void BindPlayer()
    {
        Debug.Log("bind player");
        ////joystickNames = Input.GetJoystickNames();
        if (numOfPlayers == 0)
        {
           
            player1.PlayerType = PlayerType.Player1;
            //player1.Controller = joystickNames[0];
        }
        else
        {
            player2.PlayerType = PlayerType.Player2;
            //player2.Controller = joystickNames[1];
        }
        numOfPlayers++;
        CheckNumOfPlayers();
    }
    //public IEnumerator CheckControllers()
    //{
    //    joystickNames = Input.GetJoystickNames();
    //    if (joystickNames.Count() != 2)
    //    {
    //        resetConnection = true;
    //        StopCoroutine(CheckControllers());
    //    }        
    //    yield return new WaitForSecondsRealtime(1f);
    //}
}
