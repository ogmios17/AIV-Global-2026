using UnityEngine;

[CreateAssetMenu(fileName = "MiniSequenceState", menuName = "Scriptable Objects/MiniSequenceState")]
public class MiniSequenceState : ScriptableObject, IState
{
    private Jammer player1;
    private Jammer player2;

    public GameObject prefab;
    [SerializeField] private Vector3 spawnOffset = new Vector3(0, -14, -1);
    private SequenceHandler handler;
    private GameObject prefabClone;
    

    public SequenceHandler Handler { get { return handler; } }
    public void OnStateEnter()
    {
        GlobalData.Instance.text.SetTextMessage("The kaijus dive in hell!");

        player1 = GlobalData.Instance.Player1;
        player2 = GlobalData.Instance.Player2;

        // Posiziono il prefab del minigioco rispetto al root del Fight (vedi FightInstanceManager).
        Vector3 position = Vector3.zero;
        Transform fight = GlobalData.Instance.miniGameTransform;
        if (fight != null)
            position = fight.position + spawnOffset;

        prefabClone = Instantiate(prefab, position, Quaternion.identity);
        handler = prefabClone.GetComponent<SequenceHandler>();

        player1.Input.gameObject.GetComponent<PlayerSequenceInput>().enabled = true;
        
        if (!player2.IsCPUMode)
            player2.Input.gameObject.GetComponent<PlayerSequenceInput>().enabled = true;
    }

    public void OnStateExit()
    {
        player1.Input.gameObject.GetComponent<PlayerSequenceInput>().enabled = false;
        
        if (!player2.IsCPUMode)
            player2.Input.gameObject.GetComponent<PlayerSequenceInput>().enabled = false;
        
        // Distruggi il prefab del minigioco e resetta l'handler
        if (prefabClone != null)
        {
            GameObject.Destroy(prefabClone);
            prefabClone = null;
        }
        handler = null;
    }

    public void OnStateStay()
    {

    }

    public void OnFixedStateStay()
    {

    }
}
