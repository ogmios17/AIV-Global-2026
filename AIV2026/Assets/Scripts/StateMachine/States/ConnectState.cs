using System.Collections;
using UnityEngine;

public class ConnectState : BaseState
{
    public override void OnStateEnter()
    {
        // ControllerHandler.instance.StartLookingForControllers();
        ControllerHandler.instance.CheckNumOfPlayers();
       
    }

    public override void OnStateExit()
    {
        //ControllerHandler.instance.StopLookingForControllers();
    }

    
}
