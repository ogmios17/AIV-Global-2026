using UnityEngine;

public interface StateInterface 
{
    void OnStateEnter();
    void OnStateExit();
    void OnStateStay();
    void OnFixedStateStay();
}
