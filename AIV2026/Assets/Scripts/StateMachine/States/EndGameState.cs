using UnityEngine;

[CreateAssetMenu(fileName = "EndGameState", menuName = "Scriptable Objects/EndGameState")]
public class EndGameState : ScriptableObject, IState
{
    private const float CreditsDuration = 100.0f;
    private float timer;

    // Credits lines shown as the timer counts down. The first line whose threshold is
    // >= the current time wins (lowest threshold first). Above the last threshold the
    // defeat flavor text is shown instead.
    private static readonly (int maxTime, string message)[] CreditsLines =
    {
        (40, "Thanks for playing our masterpiece!"),
        (50, "I hope you at least enjoyed the kissing scene <3"),
        (60, "We developers didn't sleep for 2 days :("),
        (70, "We all work day jobs!"),
        (80, "Only five days, what did you expect?"),
        (90, "Still here? The game is Over!"),
    };

    public void OnStateEnter()
    {
        timer = CreditsDuration;
    }

    public void OnStateStay()
    {
        timer -= Time.deltaTime;

        if (GlobalData.Instance.text == null) return;

        int parsedTimer = Mathf.CeilToInt(timer);
        foreach (var line in CreditsLines)
        {
            if (parsedTimer <= line.maxTime)
            {
                GlobalData.Instance.text.SetTextMessage(line.message);
                return;
            }
        }

        // Still before the credits roll: show who lost.
        Jammer loser = GetLoser();
        if (loser != null)
            GlobalData.Instance.text.SetTextMessage(CharacterFlavor.DefeatMessage(loser.CharacterType));
    }

    private static Jammer GetLoser()
    {
        if (GlobalData.Instance.Player1.IsDead) return GlobalData.Instance.Player1;
        if (GlobalData.Instance.Player2.IsDead) return GlobalData.Instance.Player2;
        return null;
    }

    public void OnStateExit() { }
    public void OnFixedStateStay() { }
}
