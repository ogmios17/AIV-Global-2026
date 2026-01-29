using UnityEngine;

[CreateAssetMenu(fileName = "MiniMashState", menuName = "Scriptable Objects/MiniMashState")]
public class MiniMashState : ScriptableObject, StateInterface
{
    private Jammer player1;
    private Jammer player2;
    public delegate void OnP1Mash();
    public delegate void OnP2Mash();
    public void OnStateEnter()
    {
        player1 = GlobalData.Instance.Player1;
        player2 = GlobalData.Instance.Player2;

        player1.Input.SwitchCurrentActionMap("Mash");
        player2.Input.SwitchCurrentActionMap("Mash");
    }

    public void OnStateExit()
    {

    }

    public void OnStateStay()
    {

    }

    public void OnFixedStateStay()
    {

    }

    public void Onp1Mash()
    {
        Debug.Log("A bucchin i sort");
    }

    public void Onp2Mash()
    {
        Debug.Log("A bucchin i mammt");
    }
}
