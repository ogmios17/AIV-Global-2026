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

public class ControllerHandler : MonoBehaviour
{  
    string[] joystickNames;
    public static ControllerHandler instance;
    private Jammer player1;
    private Jammer player2;
    private bool bindingComplete;
    private bool resetConnection;
    private int numOfPlayers = 0;
    private PlayerInputManager playerInput;
    public bool BindingComplete { get => bindingComplete; }
    public bool ResetConnection { get => resetConnection; }
    public Jammer Player1 { get => player1; }
    public Jammer Player2 { get => player2; }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        player1 = new Jammer();
        player2 = new Jammer();
        
        resetConnection = false;
    }

    private void Start()
    {
        playerInput = gameObject.GetComponent<PlayerInputManager>();
    }
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

    public void CheckNumOfPlayers()
    {
        if (playerInput.playerCount > 1)
        {
            bindingComplete = true;
        }
    }
    public void BindPlayer()
    {
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
