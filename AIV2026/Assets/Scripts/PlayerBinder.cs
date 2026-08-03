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

        // Ogni PlayerInput ha la PROPRIA lista di device accoppiati: sempre devices[0].
        // L'evento controller va invocato DOPO aver pubblicato il Jammer su GlobalData,
        // perché i listener (Callout) leggono GlobalData.Instance.PlayerX.Controller.
        if (input.playerIndex == 0)
        {
            Jammer.PlayerType = PlayerType.Player1;
            Jammer.Controller = input.devices[0];
            Jammer.CharacterType = character;
            GlobalData.Instance.Player1 = Jammer;
            GlobalData.Instance.onP1ControllerChosen?.Invoke();
        }
        else if (input.playerIndex == 1)
        {
            Jammer.PlayerType = PlayerType.Player2;
            Jammer.Controller = input.devices[0];
            Jammer.CharacterType = character;
            GlobalData.Instance.Player2 = Jammer;
            GlobalData.Instance.onP2ControllerChosen?.Invoke();
        }
        DontDestroyOnLoad(gameObject);
    }
}
