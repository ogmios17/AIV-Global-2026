using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    private static TransitionManager instance;
    public static TransitionManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindFirstObjectByType<TransitionManager>();
            return instance;
        }
    }

    public TransitionLayer TransitionLayer { get; private set; }

    void Awake()
    {
        TransitionLayer = GetComponentInChildren<TransitionLayer>();
    }

    void Start()
    {
        if (!TransitionLayer.isDone)
            TransitionLayer.HideImmediately();
    }
}
