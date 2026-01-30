using UnityEngine;

[CreateAssetMenu(fileName = "MiniSequenceState", menuName = "Scriptable Objects/MiniSequenceState")]
public class MiniSequenceState : ScriptableObject
{
    public GameObject prefab;
    private SequenceHandler handler;
    private GameObject prefabClone;
    

    public SequenceHandler Handler { get { return handler; } }
    public void OnStateEnter()
    {
        prefabClone = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        handler = prefabClone.GetComponent<SequenceHandler>();
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
