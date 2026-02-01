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

        // Prendo il transform del GameObject "Fight" e ci aggiungo il prefab del minigioco
        Vector3 position = Vector3.zero;
        GameObject fight = GameObject.Find("Fight");
        if(fight != null)
            position = fight.transform.position + new Vector3(0, -12, -1);

        prefabClone = Instantiate(prefab, position, Quaternion.identity);
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

        // Distruggi il prefab del minigioco e resetta l'handler
        Debug.Log("OnStateExit");
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
