using UnityEngine;

[CreateAssetMenu(fileName = "MiniMashState", menuName = "Scriptable Objects/MiniMashState")]
public class MiniMashState : ScriptableObject, StateInterface
{
    public Jammer player1;
    public Jammer player2;

    public GameObject prefab;
    private MashHandler handler;
    private GameObject prefabClone;

    public MashHandler Handler { get { return handler; } }
    public void OnStateEnter()
    {
        player1 = GlobalData.Instance.Player1;
        player2 = GlobalData.Instance.Player2;

        prefabClone = Instantiate(prefab, new Vector3(0,0,0),Quaternion.identity);
        handler = prefabClone.GetComponent<MashHandler>();

        player1.Input.gameObject.GetComponent<PlayerMashScript>().enabled = true;

        if (!player2.IsCPUMode)
            player2.Input.gameObject.GetComponent<PlayerMashScript>().enabled = true;
    }

    public void OnStateExit()
    {
        player1.Input.gameObject.GetComponent<PlayerMashScript>().enabled = false;

        if (!player2.IsCPUMode)
            player2.Input.gameObject.GetComponent<PlayerMashScript>().enabled = false;
    }

    public void OnStateStay()
    {

    }

    public void OnFixedStateStay()
    {
        
    }

    
}
