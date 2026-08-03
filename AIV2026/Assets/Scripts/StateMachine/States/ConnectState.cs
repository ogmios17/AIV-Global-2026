using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ConnectState", menuName = "Scriptable Objects/ConnectState")]
public class ConnectState : ScriptableObject, IState
{
   
    public void OnStateEnter()
    {
        
    }

    public void OnStateExit()
    {
       
    }


    public void OnStateStay() { }
    public void OnFixedStateStay() { }
}
