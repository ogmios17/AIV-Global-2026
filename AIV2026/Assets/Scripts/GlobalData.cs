using UnityEngine;

public class GlobalData : MonoBehaviour
{
    private Jammer player1;
    private Jammer player2;

    private static GlobalData instance;

    public static GlobalData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GlobalData();
            }
            return instance;
        }
    }

    public Jammer Player1
    {
        get
        {
            if (player1 == null)
                player1 = new Jammer();
            return player1;
        }
        set => player1 = value;
    }
    public Jammer Player2
    {
        get
        {
            if (player2 == null)
                player2 = new Jammer();
            return player2;
        }
        set => player2 = value;
    }
}
