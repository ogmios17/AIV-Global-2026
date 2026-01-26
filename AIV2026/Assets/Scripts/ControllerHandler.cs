using System.Collections;
using System.Linq;
using UnityEngine;

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
        resetConnection = false;
    }

    public void StartLookingForControllers()
    {
        resetConnection = false;
        bindingComplete = false;
        StartCoroutine(GetConnectedControllers());
    }

    public void StopLookingForControllers()
    {
        StopCoroutine(GetConnectedControllers());
    }

    public IEnumerator GetConnectedControllers()
    {
        joystickNames = Input.GetJoystickNames();
        if (joystickNames.Count() != 2)
        {
            yield return new WaitForSecondsRealtime(2f);
        }
        player1.Controller = joystickNames[0];
        player2.Controller = joystickNames[1];
        bindingComplete = true;
    }

    public IEnumerator CheckControllers()
    {
        joystickNames = Input.GetJoystickNames();
        if (joystickNames.Count() != 2)
        {
            resetConnection = true;
            StopCoroutine(CheckControllers());
        }        
        yield return new WaitForSecondsRealtime(1f);
    }
}
