using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerBinder : MonoBehaviour
{
    public Jammer Jammer { get; private set; }

    private void Awake()
    {
        var input = GetComponent<PlayerInput>();
        Jammer = new Jammer();
        Jammer.Input = input;

        if (input.playerIndex == 0)
        {
            Jammer.PlayerType = PlayerType.Player1;
            GlobalData.Instance.Player1 = Jammer;
        }
        else if (input.playerIndex == 1)
        {
            Jammer.PlayerType = PlayerType.Player2;
            GlobalData.Instance.Player2 = Jammer;
        }

    }
}
