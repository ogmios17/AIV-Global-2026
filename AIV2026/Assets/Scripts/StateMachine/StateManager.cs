using UnityEngine;

public class StateManager : MonoBehaviour
{
    private StateMachine gameStateMachine;
    public static StateManager instance;

    private void Start()
    {
        
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        gameStateMachine = new StateMachine();
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
    

}
