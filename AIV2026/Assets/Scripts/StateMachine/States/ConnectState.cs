using System.Collections;
using UnityEngine;

public class ConnectState : BaseState
{
    private ControllerHandler controllerHandler;
    public override void OnStateEnter()
    {
        // ControllerHandler.instance.StartLookingForControllers();
        controllerHandler.CheckNumOfPlayers();
       
    }

    public override void OnStateExit()
    {
        //ControllerHandler.instance.StopLookingForControllers();
    }

    public ConnectState(ControllerHandler controllerHandler)
    {
        this.controllerHandler = controllerHandler;
    }
}
