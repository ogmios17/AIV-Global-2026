using UnityEngine;

[CreateAssetMenu(fileName = "StartGameState", menuName = "Scriptable Objects/StartGameState")]
public class StartGameState : ScriptableObject, IState
{
    public void OnStateEnter() { }
    public void OnStateExit() { }
    public void OnStateStay() { }
    public void OnFixedStateStay() { }
}

