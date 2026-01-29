using UnityEngine;

[CreateAssetMenu(fileName = "MiniMashState", menuName = "Scriptable Objects/MiniMashState")]
public class MiniMashState : ScriptableObject, StateInterface
{
    public GameObject prefab;
    private MashHandler handler;

    public MashHandler Handler { get { return handler; } }
    public void OnStateEnter()
    {
        Instantiate(prefab, new Vector3(0,0,0),Quaternion.identity);
        handler = prefab.GetComponent<MashHandler>();
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
