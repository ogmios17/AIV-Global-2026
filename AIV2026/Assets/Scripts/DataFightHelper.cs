using UnityEngine;

public class DataFightHelper : MonoBehaviour
{
    private static DataFightHelper instance;

    public static DataFightHelper Instance { get => instance; set => instance = value; }
    public GameObject Fader { get => fader; set => fader = value; }
    public GameObject P1popUp { get => p1popUp; set => p1popUp = value; }
    public GameObject P2popUp { get => p2popUp; set => p2popUp = value; }

    [SerializeField] private GameObject fader;
    [SerializeField] private GameObject p1popUp;
    [SerializeField] private GameObject p2popUp;

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
