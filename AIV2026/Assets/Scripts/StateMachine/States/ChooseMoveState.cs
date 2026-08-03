using System.Collections.Generic;
using UnityEngine;
public enum CardTypes
{
    Attack,
    Block,
    Shove,
    Grapple
}

[CreateAssetMenu(fileName = "ChooseMoveState", menuName = "Scriptable Objects/ChooseMoveState")]
public class ChooseMoveState : ScriptableObject, IState
{
    [Header("Timer")]
    private bool timerActive;
    private float timer;
    public List<string> encouragementSentences;
    public List<string> clashSentences;
    public List<string> tooSlowSentences;

    private Jammer player1;
    private Jammer player2;
    public GameObject prefab;
    private int nextMinigame;
    private bool clashSentencePerformed;

    [Header("Available Move Cards")]
    public MoveCard[] availableMoves;

    public int NextMinigame { get => nextMinigame; }

    public void OnStateEnter()
    {
        // Setto la scelta delle carte dei giocatori a null (quando entrano in choosemove ancora non hanno scelto nulla)
        GlobalData.Instance.Player1.ChosenMove = null;
        GlobalData.Instance.Player2.ChosenMove = null;

        timer = 10f;
        timerActive = true;
        nextMinigame = -1;
        clashSentencePerformed = false;

        player1 = GlobalData.Instance.Player1;
        player2 = GlobalData.Instance.Player2;

        // Human P1
        player1.Input.gameObject.GetComponent<PlayerMoveInput>().enabled = true;
        player1.Input.SwitchCurrentActionMap(ActionMaps.CardSelection);

        // Human P2
        if (!player2.IsCPUMode)
        {
            player2.Input.gameObject.GetComponent<PlayerMoveInput>().enabled = true;
            player2.Input.SwitchCurrentActionMap(ActionMaps.CardSelection);
        }
    }

    public void OnStateStay()
    {
        // CPU sceglie mossa random se non l'ha già fatto
        if (player2.IsCPUMode && player2.ChosenMove == null && timer<=7)
        {
            GlobalData.Instance.Player2.ChosenMove = SelectRandomMoveCard();
            PlayMoveAnimation(player2);
        }

        // Controllo se entrambi i giocatori hanno risposto
        MoveCard P1Move = GlobalData.Instance.Player1.ChosenMove;
        MoveCard P2Move = GlobalData.Instance.Player2.ChosenMove;
        if(P1Move != null && P2Move != null)
            Resolve(P1Move, P2Move);

        if (timerActive)
        {
            timer -= Time.deltaTime;
            switch (timer)
            {
                case <= 0:
                    GlobalData.Instance.text.SetTextMessage("Time's up!");

                    // Scegli una carta casuale per chi non ha ancora giocato
                    if (GlobalData.Instance.Player1.ChosenMove == null)
                    {
                        GlobalData.Instance.Player1.ChosenMove = SelectRandomMoveCard();
                        PlayMoveAnimation(player1);
                        if (tooSlowSentences.Count > 0)
                        {
                            GlobalData.Instance.text.SetTextMessage(tooSlowSentences[Random.Range(0, tooSlowSentences.Count)]);
                        }
                    }

                    if (GlobalData.Instance.Player2.ChosenMove == null)
                    {
                        GlobalData.Instance.Player2.ChosenMove = SelectRandomMoveCard();
                        PlayMoveAnimation(player2);
                        if (tooSlowSentences.Count > 0)
                        {
                            GlobalData.Instance.text.SetTextMessage(tooSlowSentences[Random.Range(0, tooSlowSentences.Count)]);
                        }
                    }
                    break;

                default:
                    if(GlobalData.Instance.text)
                        GlobalData.Instance.text.SetCountDownMessage(Mathf.CeilToInt(timer).ToString());
                    break;
            }
        }
    }

    /// <summary>
    /// Triggers the chosen move's animation on both the cards and the fighter rig.
    /// MoveCard.cardName matches the animator trigger names (see AnimTriggers).
    /// </summary>
    private void PlayMoveAnimation(Jammer player)
    {
        string trigger = player.ChosenMove.cardName;
        player.CardsAnim.SetTrigger(trigger);
        player.FighterAnim.SetTrigger(trigger);
    }

    private void Resolve(MoveCard P1Move, MoveCard P2Move)
    {
        timer = 10f;
        timerActive = false;

        player1.onMoveChosen?.Invoke();
        player2.onMoveChosen?.Invoke();

        if(timer>=0 && encouragementSentences.Count>0)
            GlobalData.Instance.text.SetTextMessage(encouragementSentences[Random.Range(0, encouragementSentences.Count)]);

        player1.CardsAnim.SetTrigger(AnimTriggers.Reveal);
        player2.CardsAnim.SetTrigger(AnimTriggers.Reveal);

        if (P1Move.draws.Contains(P2Move))
        {
            timerActive = true;
            player2.FighterAnim.SetTrigger(AnimTriggers.Next);
            player1.FighterAnim.SetTrigger(AnimTriggers.Next);
            GainMana(player1, 1);
            GainMana(player2, 1);

            AudioManager.Instance.PlayCancelCard();
            AudioManager.Instance.PlayCrowdPanic(1f);
            GlobalData.Instance.Player1.ChosenMove = null;
            GlobalData.Instance.Player2.ChosenMove = null;
        }
        else if (P1Move.clashes.Contains(P2Move))
        {
            player2.FighterAnim.SetTrigger(AnimTriggers.Next);
            player1.FighterAnim.SetTrigger(AnimTriggers.Next);
            AudioManager.Instance.PlayCancelCard();
            AudioManager.Instance.PlayCrowdPanic(1f);
            ChooseMinigame();
        }
        else if (P1Move.wins == P2Move)
        {
            // Nota: i trigger "Next" solo in questo ramo, come nel codice originale.
            player2.FighterAnim.SetTrigger(AnimTriggers.Next);
            player1.FighterAnim.SetTrigger(AnimTriggers.Next);
            ResolveRound(winner: player1, loser: player2, winningMove: P1Move);
        }
        else if (P1Move.loses == P2Move)
        {
            ResolveRound(winner: player2, loser: player1, winningMove: P2Move);
        }

        GlobalData.Instance.Player1.CardsAnim.SetTrigger(AnimTriggers.Out);
        GlobalData.Instance.Player2.CardsAnim.SetTrigger(AnimTriggers.Out);
    }

    /// <summary>Applies the outcome of a won round: damage, mana (2 winner / 1 loser), audio, ability events.</summary>
    private void ResolveRound(Jammer winner, Jammer loser, MoveCard winningMove)
    {
        timerActive = true;
        loser.FighterAnim.SetTrigger(AnimTriggers.Damage);
        loser.TakeAHit();
        GainMana(winner, 2);
        GainMana(loser, 1);
        AudioManager.Instance.UpdateCombatMusicByHealth(player1.Health, player2.Health);
        AudioManager.Instance.CheckLastHP(player1.Health, player2.Health);
        AudioManager.Instance.PlayCardSound(winningMove);
        AudioManager.Instance.PlayCrowdPanic(1f);

        winner.onMoveHits?.Invoke();
        loser.onMoveMisses?.Invoke();

        GlobalData.Instance.Player1.ChosenMove = null;
        GlobalData.Instance.Player2.ChosenMove = null;
    }

    private void GainMana(Jammer player, int amount)
    {
        player.CharacterPrefab.GetComponent<FightersDataBinder>().GainMana(amount, player);
    }

    public void OnFixedStateStay()
    {

    }

    public void OnStateExit()
    {
        player1.Input.gameObject.GetComponent<PlayerMoveInput>().enabled = false;
        if (!player2.IsCPUMode)
            player2.Input.gameObject.GetComponent<PlayerMoveInput>().enabled = false;
    }

    public void ChooseMinigame()
    {
        int index = Random.Range(0, 2);
        nextMinigame = index;
    }

    public void OnP1Received(MoveCard move)
    {
        if (timer > 7) return;
        GlobalData.Instance.Player1.ChosenMove = move;
        PlayMoveAnimation(player1);
    }

    public void OnP2Received(MoveCard move)
    {
        if (timer > 7) return;
        GlobalData.Instance.Player2.ChosenMove = move;
        PlayMoveAnimation(player2);
    }

    public void TryUseAbility(Jammer player)
    {
        int cost = GlobalData.Instance.Characters[(int)player.CharacterType].abilityCost;
        if (player.Mana >= cost && !player.CharacterLogic.AbilityTriggeredThisTurn)
        {
            player.CharacterLogic.TriggerAbility();
            player.CharacterPrefab.GetComponent<FightersDataBinder>().UseMana(cost, player);
        }
    }

    public void TryUseUlti(Jammer player)
    {
        int cost = GlobalData.Instance.Characters[(int)player.CharacterType].ultCost;
        if (player.Mana >= cost && !player.CharacterLogic.UltiTriggeredThisTurn)
        {
            player.CharacterLogic.TriggerUlt();
            player.CharacterPrefab.GetComponent<FightersDataBinder>().UseMana(cost, player);
        }
    }

    private MoveCard SelectRandomMoveCard()
    {
        int randomIndex = Random.Range(0, availableMoves.Length);
        return availableMoves[randomIndex];
    }
}
