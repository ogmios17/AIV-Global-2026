using UnityEngine;

public interface IState 
{
    void OnStateEnter();
    void OnStateExit();
    void OnStateStay();
    void OnFixedStateStay();
}
