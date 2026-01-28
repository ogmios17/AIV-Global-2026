using UnityEngine;

[CreateAssetMenu(fileName = "IdelState", menuName = "Scriptable Objects/IdleState")]
public class IdleState : ScriptableObject, StateInterface
{
    public void OnStateEnter() { }
    public void OnStateExit() { }
    public void OnStateStay() { }
    public void OnFixedStateStay() { }
}
