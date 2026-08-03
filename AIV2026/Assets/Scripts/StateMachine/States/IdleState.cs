using UnityEngine;

[CreateAssetMenu(fileName = "IdleState", menuName = "Scriptable Objects/IdleState")]
public class IdleState : ScriptableObject, IState
{
    private CharacterSelectionInputManager handler;
    public GameObject prefab;
    private GameObject prefabClone;

    public CharacterSelectionInputManager Handler {  get { return handler; } }

    public void OnStateEnter() {
        prefabClone = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        handler = prefabClone.GetComponentInChildren<CharacterSelectionInputManager>();
        handler.IsCPUMode = PlayerPrefs.GetInt("IsCPUMode") == 1;
    }
    public void OnStateExit() { }
    public void OnStateStay() { }
    public void OnFixedStateStay() { }
}
