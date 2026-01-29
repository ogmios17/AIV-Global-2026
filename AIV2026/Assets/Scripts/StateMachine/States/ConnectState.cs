using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ConnectState", menuName = "Scriptable Objects/ConnectState")]
public class ConnectState : ScriptableObject, StateInterface
{
   
    public void OnStateEnter()
    {
        Debug.Log("Waiting for players...");
        
    }

    public void OnStateExit()
    {
       
    }


    public void OnStateStay() { }
    public void OnFixedStateStay() { }
}
