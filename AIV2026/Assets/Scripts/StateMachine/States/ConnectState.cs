using System.Collections;
using UnityEngine;

public class ConnectState : BaseState
{
    public override void OnStateEnter()
    {
        ControllerHandler.instance.StartLookingForControllers();
    }

    public override void OnStateExit()
    {
        //ControllerHandler.instance.StopLookingForControllers();
    }

    
}
