using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

[CreateAssetMenu(fileName = "IdelState", menuName = "Scriptable Objects/IdleState")]
public class IdleState : ScriptableObject, StateInterface
{
    public Jammer player1;
    public Jammer player2;
    private CharacterSelectionInputManager handler;
    public GameObject prefab;
    private GameObject prefabClone;

    public CharacterSelectionInputManager Handler {  get { return handler; } }

    public void OnStateEnter() {
        player1 = GlobalData.Instance.Player1;
        player2 = GlobalData.Instance.Player2;

        prefabClone = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        handler = prefabClone.GetComponentInChildren<CharacterSelectionInputManager>();
        handler.IsCPUMode = PlayerPrefs.GetInt("IsCPUMode") == 1;
        
        //player1.Input.gameObject.GetComponent<PlayerUIInput>().enabled = true;
        //player2.Input.gameObject.GetComponent<PlayerUIInput>().enabled = true;
    }
    public void OnStateExit() {
        //player1.Input.gameObject.GetComponent<PlayerUIInput>().enabled = false;
        //player2.Input.gameObject.GetComponent<PlayerUIInput>().enabled = false;
    }
    public void OnStateStay() { }
    public void OnFixedStateStay() { }
}
