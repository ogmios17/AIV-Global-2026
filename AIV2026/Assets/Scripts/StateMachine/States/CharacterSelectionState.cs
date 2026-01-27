using UnityEngine;

public class CharacterSelectionState : BaseState
{
    public override void OnStateEnter()
    {
        ControllerHandler.instance.StartLookingForControllers();
    }
}
