using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerBinder : MonoBehaviour
{
    public int playerIndex;
    public Jammer Jammer { get; private set; }
    public CharacterType character;
    public CharacterType Character { get { return character; } set => character = value; }

    private void Awake()
    {
        var input = GetComponent<PlayerInput>();
        Jammer = new Jammer();
        Jammer.Input = input;

        if (input.playerIndex == 0)
        {
            Jammer.PlayerType = PlayerType.Player1;
            Jammer.Controller = input.devices[0];
            GlobalData.Instance.onP1ControllerChosen?.Invoke();
            Jammer.CharacterType = character;
            Debug.Log("controller: " + Jammer.Controller);
            GlobalData.Instance.Player1 = Jammer;      
        }
        else if (input.playerIndex == 1)
        {
            Jammer.PlayerType = PlayerType.Player2;
            GlobalData.Instance.onP1ControllerChosen?.Invoke();
            Jammer.CharacterType = character;
            Jammer.Controller = input.devices[1];
            Debug.Log("controller: " + Jammer.Controller);
            GlobalData.Instance.Player2 = Jammer;
        }
        DontDestroyOnLoad(gameObject);
    }
}
