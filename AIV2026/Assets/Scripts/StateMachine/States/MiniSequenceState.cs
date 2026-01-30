using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

[CreateAssetMenu(fileName = "MiniSequenceState", menuName = "Scriptable Objects/MiniSequenceState")]
public class MiniSequenceState : ScriptableObject, StateInterface
{
    public Jammer player1;
    public Jammer player2;

    public GameObject prefab;
    private SequenceHandler handler;
    private GameObject prefabClone;
    

    public SequenceHandler Handler { get { return handler; } }
    public void OnStateEnter()
    {
        player1 = GlobalData.Instance.Player1;
        player2 = GlobalData.Instance.Player2;

        prefabClone = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        handler = prefabClone.GetComponent<SequenceHandler>();

        player1.Input.gameObject.GetComponent<PlayerSequenceInput>().enabled = true;
        player2.Input.gameObject.GetComponent<PlayerSequenceInput>().enabled = true;
    }

    public void OnStateExit()
    {
        player1.Input.gameObject.GetComponent<PlayerSequenceInput>().enabled = false;
        player2.Input.gameObject.GetComponent<PlayerSequenceInput>().enabled = false;
    }

    public void OnStateStay()
    {

    }

    public void OnFixedStateStay()
    {

    }
}
