using JetBrains.Annotations;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    private StateMachine gameStateMachine;

    [SerializeField]
    private ControllerHandler controllerHandler;
    [SerializeField]
    private ConnectState connectState;
    [SerializeField]
    private ChooseMoveState chooseMoveState;
    [SerializeField]
    private CharacterSelectionState characterSelectionState;
    [SerializeField]
    private IdleState idleState;
    [SerializeField]
    private StartGameState startGameState;
    [SerializeField]
    private MiniMashState miniMashState;
    [SerializeField]
    private MiniSequenceState miniSequenceState;
    private Jammer player1;
    private Jammer player2;

    //public Jammer Player1 { get => player1; set => player1 = value; }
    //public Jammer Player2 { get => player2; set => player2 = value; }

    public MiniMashState MiniMashState {  get { return miniMashState; } }
    public MiniSequenceState MiniSequenceState { get { return miniSequenceState; } }

    private void Start()
    {
        gameStateMachine = new StateMachine();
        DontDestroyOnLoad(this);
        //connectState = new ConnectState(controllerHandler);
        //chooseMoveState = new ChooseMoveState();
        gameStateMachine.AddTransition(connectState, chooseMoveState,
            new FuncPredicate(() => controllerHandler.BindingComplete ));
        gameStateMachine.AddTransition( chooseMoveState, miniMashState,
            new FuncPredicate(() => chooseMoveState.goToMinigame));

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
