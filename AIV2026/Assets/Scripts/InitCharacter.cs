using UnityEngine;

public class InitCharacter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GlobalData.Instance.stateManager.GoNext();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
