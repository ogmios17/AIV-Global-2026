using UnityEngine;
public class GlobalData : MonoBehaviour
{

    public static GlobalData Instance { get; private set; }
    public StateManager stateManager;

    public Jammer Player1;
    public Jammer Player2;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
  
}
