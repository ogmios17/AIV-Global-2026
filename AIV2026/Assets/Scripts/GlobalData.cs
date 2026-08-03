using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
public class GlobalData : MonoBehaviour
{
    [SerializeField] private List<CharacterSO> characters;
    public static GlobalData Instance { get; private set; }
    public List<CharacterSO> Characters { get => characters; set => characters = value; }
    
    public StateManager stateManager;
    public Transform miniGameTransform;
    public TextSetter text;

    public Jammer Player1;
    public Jammer Player2;

    public Action onP1ControllerChosen;
    public Action onP2ControllerChosen;

    public Jammer GetOpponent(Jammer player) => player == Player1 ? Player2 : Player1;

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
