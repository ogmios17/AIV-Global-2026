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

        if (playerIndex == 1)
        {
            Jammer.PlayerType = PlayerType.Player1;
            Jammer.Controller = input.currentControlScheme;
            Jammer.CharacterType = character;
            Debug.Log("controller: " + Jammer.Controller);
            GlobalData.Instance.Player1 = Jammer;
        }
        else if (playerIndex == 2)
        {
            Jammer.PlayerType = PlayerType.Player2;
            Jammer.Controller = input.currentControlScheme;
            Jammer.CharacterType = character;
            Debug.Log("controller: " + Jammer.Controller);
            GlobalData.Instance.Player2 = Jammer;
        }
        DontDestroyOnLoad(gameObject);
    }
}
