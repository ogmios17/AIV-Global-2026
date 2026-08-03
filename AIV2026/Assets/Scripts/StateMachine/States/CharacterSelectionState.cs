using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSelectionState", menuName = "Scriptable Objects/CharacterSelectionState")]
public class CharacterSelectionState : ScriptableObject, IState
{

    public void OnStateEnter() {
        //ControllerHandler.instance.StartLookingForControllers();
    }
    public void OnStateExit() { }
    public void OnStateStay() { }
    public void OnFixedStateStay() { }
}
