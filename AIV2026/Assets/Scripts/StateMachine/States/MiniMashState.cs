using UnityEngine;

[CreateAssetMenu(fileName = "MiniMashState", menuName = "Scriptable Objects/MiniMashState")]
public class MiniMashState : ScriptableObject, StateInterface
{
    public GameObject prefab;
    private MashHandler handler;
    private GameObject prefabClone;

    public MashHandler Handler { get { return handler; } }
    public void OnStateEnter()
    {
        prefabClone = Instantiate(prefab, new Vector3(0,0,0),Quaternion.identity);
        handler = prefabClone.GetComponent<MashHandler>();
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

    
}
