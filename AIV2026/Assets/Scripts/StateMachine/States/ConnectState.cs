using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ConnectState", menuName = "Scriptable Objects/ConnectState")]
public class ConnectState : ScriptableObject, StateInterface
{
    private ControllerHandler controllerHandler;
    public void OnStateEnter()
    {
        // ControllerHandler.instance.StartLookingForControllers();
        controllerHandler.CheckNumOfPlayers();
       
    }

    public void OnStateExit()
    {
        //ControllerHandler.instance.StopLookingForControllers();
    }

    public ConnectState(ControllerHandler controllerHandler)
    {
        this.controllerHandler = controllerHandler;
    }

    public void OnStateStay() { }
    public void OnFixedStateStay() { }
}
