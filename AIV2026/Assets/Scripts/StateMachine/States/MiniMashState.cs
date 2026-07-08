using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MiniMashState", menuName = "Scriptable Objects/MiniMashState")]
public class MiniMashState : ScriptableObject, IState
{
    private Jammer player1;
    private Jammer player2;

    public GameObject prefab;
    [SerializeField] private Vector3 spawnOffset = new Vector3(0, -12, -1);
    private MashHandler handler;
    private GameObject prefabClone;
    public List<string> loveSentences;
    public MashHandler Handler { get { return handler; } }
    public void OnStateEnter()
    {
        GlobalData.Instance.text.SetTextMessage("The kaijus ERUPT in a passionate manifestation of love!");

        player1 = GlobalData.Instance.Player1;
        player2 = GlobalData.Instance.Player2;

        // Posiziono il prefab del minigioco rispetto al root del Fight (vedi FightInstanceManager).
        Vector3 position = Vector3.zero;
        Transform fight = GlobalData.Instance.miniGameTransform;
        if (fight != null)
            position = fight.position + spawnOffset;

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
