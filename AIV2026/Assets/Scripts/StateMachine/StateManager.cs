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
    private EndGameState endGameState;
    [SerializeField]
    private MiniMashState miniMashState;
    [SerializeField]
    private MiniSequenceState miniSequenceState;
    private bool nextRequested;

    public MiniMashState MiniMashState {  get { return miniMashState; } }
    public MiniSequenceState MiniSequenceState { get { return miniSequenceState; } }
    public IdleState IdleState { get { return idleState; } set { idleState = value; } }
    public ChooseMoveState ChooseMoveState { get { return chooseMoveState; } }
    // public EndGameState EndGameState { get { return endGameState; } }

    private void Start()
    {
        gameStateMachine = new StateMachine();
        DontDestroyOnLoad(this);
        //chooseMoveState = new ChooseMoveState();
        gameStateMachine.AddTransition( idleState, chooseMoveState,
        new FuncPredicate(() => idleState.Handler.ChoicesDone));
        gameStateMachine.AddTransition(startGameState, idleState,
        new FuncPredicate(() => nextRequested));
        gameStateMachine.AddTransition( chooseMoveState, miniMashState,
            new FuncPredicate(() => chooseMoveState.NextMinigame==0));
        gameStateMachine.AddTransition(chooseMoveState, miniSequenceState,
            new FuncPredicate(() => chooseMoveState.NextMinigame == 1));
        // gameStateMachine.AddTransition(chooseMoveState, startGameState,
        //    new FuncPredicate(() => (GlobalData.Instance.Player1.Health<=0 || GlobalData.Instance.Player2.Health<=0)));
        gameStateMachine.AddTransition(chooseMoveState, endGameState,
           new FuncPredicate(() => (GlobalData.Instance.Player1.Health<=0 || GlobalData.Instance.Player2.Health<=0)));
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
            gameStateMachine.SetState(gameStateMachine.NextNode.State);
        }
    }

    public void SetNextNode(StateNode node)
    {
        gameStateMachine.NextNode = node;
    }

    /// <summary>One-shot signal that advances StartGame -> Idle.</summary>
    public void RequestNext()
    {
        nextRequested = true;
    }

}
