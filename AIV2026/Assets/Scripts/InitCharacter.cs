using UnityEngine;

public class InitCharacter : MonoBehaviour
{
    void Start()
    {
        GlobalData.Instance.stateManager.RequestNext();
    }
}
