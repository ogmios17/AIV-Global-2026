using JetBrains.Annotations;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    private StateMachine gameStateMachine;
    public static StateManager instance;

    private void Start()
    {
        ConnectState connectState = new ConnectState();
        ChooseMoveState chooseMoveState = new ChooseMoveState();
        gameStateMachine.AddTransition(connectState, chooseMoveState,
            new FuncPredicate(() => ControllerHandler.instance.BindingComplete && gameStateMachine.PreviousNode.GetType() == typeof(StartGameState)));
        gameStateMachine.AddAnyTransition(connectState,
            new FuncPredicate(() => ControllerHandler.instance.ResetConnection));
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            instance = this;
            DontDestroyOnLoad(this);
            gameStateMachine = new StateMachine();
            gameStateMachine.GetOrAddNode(new ConnectState());
            gameStateMachine.SetState(new ConnectState());
        }
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
