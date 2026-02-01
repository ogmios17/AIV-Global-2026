using JetBrains.Annotations;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    private StateMachine gameStateMachine;

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
    private bool next = false;

    //public Jammer Player1 { get => player1; set => player1 = value; }
    //public Jammer Player2 { get => player2; set => player2 = value; }

    public MiniMashState MiniMashState {  get { return miniMashState; } }
    public MiniSequenceState MiniSequenceState { get { return miniSequenceState; } }
    public IdleState IdleState { get { return idleState; } set { idleState = value; } }
    public ChooseMoveState ChooseMoveState { get { return chooseMoveState; } }

    private void Start()
    {
        gameStateMachine = new StateMachine();
        DontDestroyOnLoad(this);
        //chooseMoveState = new ChooseMoveState();
        gameStateMachine.AddTransition( idleState, chooseMoveState,
        new FuncPredicate(() => idleState.Handler._choicesDone));
        gameStateMachine.AddTransition(startGameState, idleState,
        new FuncPredicate(() => next));
        gameStateMachine.AddTransition( chooseMoveState, miniMashState,
            new FuncPredicate(() => chooseMoveState.NextMinigame==0));
        gameStateMachine.AddTransition(chooseMoveState, miniSequenceState,
            new FuncPredicate(() => chooseMoveState.NextMinigame == 1));

        // Transizioni per uscire dai minigiochi quando finiscono
        gameStateMachine.AddTransition(miniMashState, chooseMoveState,
            new FuncPredicate(() => miniMashState.Handler != null && miniMashState.Handler.IsFinished));
        gameStateMachine.AddTransition(miniSequenceState, chooseMoveState,
            new FuncPredicate(() => miniSequenceState.Handler != null && miniSequenceState.Handler.IsFinished));

        gameStateMachine.SetState(startGameState);
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

    public void GoNext()
    {
        next = true;
    }

}
