using UnityEngine;

[CreateAssetMenu(fileName = "EndGameState", menuName = "Scriptable Objects/EndGameState")]
public class EndGameState : ScriptableObject, StateInterface
{
    private float timer;

    public void OnStateEnter()
    {
        timer = 100.0f;
    }

    public void OnStateStay() {

        timer -= Time.deltaTime;

        var player1 = GlobalData.Instance.Player1;
        var player2 = GlobalData.Instance.Player2;

        if (player1.IsDead) {
            if(player1.CharacterType == CharacterType.NotZilla)
            {
                if(GlobalData.Instance.text != null)
                    GlobalData.Instance.text.SetTextMessage("Not Zilla was Godzilla all along!");
            }
            else if(player1.CharacterType == CharacterType.CrackKen)
            {
                if(GlobalData.Instance.text != null)
                    GlobalData.Instance.text.SetTextMessage("Krack Ken was a squid all along!");
            }
        }
        else if (player2.IsDead) {
            if(player2.CharacterType == CharacterType.NotZilla)
            {
                if(GlobalData.Instance.text != null)
                    GlobalData.Instance.text.SetTextMessage("Not Zilla was Godzilla all along!");
            }
            else if(player2.CharacterType == CharacterType.CrackKen)
            {
                if(GlobalData.Instance.text != null)
                    GlobalData.Instance.text.SetTextMessage("Krack Ken was a squid all along!");
            }
        }

        int parsedTimer = Mathf.CeilToInt(timer);
        switch (parsedTimer)
        {
            case <= 40:
                GlobalData.Instance.text.SetTextMessage("Thanks for playing our masterpiece!");
                break;
            case <= 50:
                GlobalData.Instance.text.SetTextMessage("I hope you at least enjoyed the kissing scene <3");
                break;
            case <= 60:
                GlobalData.Instance.text.SetTextMessage("We developers didn't sleep for 2 days :(");
                break;
            case <= 70:
                GlobalData.Instance.text.SetTextMessage("We all work day jobs!");
                break;
            case <= 80:
                GlobalData.Instance.text.SetTextMessage("Only five days, what did you expect?");
                break;
            case <= 90:
                GlobalData.Instance.text.SetTextMessage("Still here? Game is Over!");
                break;
        }
     }

    public void OnStateExit() { }
    public void OnFixedStateStay() { }
}
