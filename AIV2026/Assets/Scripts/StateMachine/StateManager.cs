using JetBrains.Annotations;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    private StateMachine gameStateMachine;
    [SerializeField]
    private ControllerHandler controllerHandler;
    private ConnectState connectState;
    private ChooseMoveState chooseMoveState;
     

    private void Start()
    {
        gameStateMachine = new StateMachine();
        DontDestroyOnLoad(this);
        connectState = new ConnectState(controllerHandler);
        chooseMoveState = new ChooseMoveState();
        gameStateMachine.AddTransition(connectState, chooseMoveState,
            new FuncPredicate(() => controllerHandler.BindingComplete ));
        gameStateMachine.AddAnyTransition(connectState,
            new FuncPredicate(() => controllerHandler.ResetConnection));


        gameStateMachine.SetState(connectState);
    }

    public void Update()
    {
        gameStateMachine.Update();
    }

    public void FixedUpdate()
    {
        gameStateMachine.FixedUpdate();
    }

    public StateNode GetCurrentNode()
    {
        return gameStateMachine.CurrentNode;
    }

    public void GoToNextState()
    {
        if (gameStateMachine.NextNode != null)
        {
            gameStateMachine.SetState(gameStateMachine.NextNode.state);
        }
    }

    public void SetNextNode(StateNode node)
    {
        gameStateMachine.NextNode = node;
    }

}
