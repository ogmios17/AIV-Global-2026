using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
public enum CardTypes
{
    Attack,
    Block,
    Shove,
    Grapple
}

[CreateAssetMenu(fileName = "ChooseMoveState", menuName = "Scriptable Objects/ChooseMoveState")]
public class ChooseMoveState : ScriptableObject, StateInterface
{
    [Header("Timer")]
    private bool timerActive;
    private float timer;
    public List<string> encouragementSentences;
    public List<string> clashSentences;
    public List<string> tooSlowSentences;

    public Jammer player1;
    public Jammer player2;
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
        player1.Input.SwitchCurrentActionMap("CardSelection");

        // Human P2
        if (!player2.IsCPUMode)
        {
            player2.Input.gameObject.GetComponent<PlayerMoveInput>().enabled = true;
            player2.Input.SwitchCurrentActionMap("CardSelection");
        }
    }

    public void OnStateStay()
    {
        // CPU sceglie mossa random se non l'ha già fatto
        if (player2.IsCPUMode && player2.ChosenMove == null && timer<=7)
        {
            // int randomIndex = Random.Range(0, 4);
            // player2.ChosenMove = availableMoves[randomIndex];

            GlobalData.Instance.Player2.ChosenMove = SelectRandomMoveCard();

            //handle animation
            HandleAnimationsP2();

            // player2.ChosenMove = availableMoves[0]; // DEBUG: Attack
            Debug.Log($"CPU sceglie: {player2.ChosenMove.cardName}");
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
                        HandleAnimationsP1();
                        if (tooSlowSentences.Count > 0)
                        {
                            GlobalData.Instance.text.SetTextMessage(tooSlowSentences[Random.Range(0, tooSlowSentences.Count)]);
                        }
                    }

                    if (GlobalData.Instance.Player2.ChosenMove == null)
                    {
                        GlobalData.Instance.Player2.ChosenMove = SelectRandomMoveCard();
                        HandleAnimationsP2();
                        if (tooSlowSentences.Count > 0)
                        {
                            GlobalData.Instance.text.SetTextMessage(tooSlowSentences[Random.Range(0, tooSlowSentences.Count)]);
                        }
                    }
                    break;

                default:
                    if(GlobalData.Instance.text)
                        GlobalData.Instance.text.SetTextMessage(Mathf.CeilToInt(timer) + " seconds left");
                    break;
            }
        }
    }

    private void HandleAnimationsP2()
    {
        switch (GlobalData.Instance.Player2.ChosenMove.cardName)
        {
            case ("Attack"):
                player2.CardsAnim.SetTrigger("Attack");
                break;
            case ("Block"):
                player2.CardsAnim.SetTrigger("Block");
                break;
            case ("Grapple"):
                player2.CardsAnim.SetTrigger("Grapple");
                break;
            case ("Shove"):
                player2.CardsAnim.SetTrigger("Shove");
                break;
        }
    }

    private void HandleAnimationsP1()
    {
        switch (GlobalData.Instance.Player1.ChosenMove.cardName)
        {
            case ("Attack"):
                player1.CardsAnim.SetTrigger("Attack");
                break;
            case ("Block"):
                player1.CardsAnim.SetTrigger("Block");
                break;
            case ("Grapple"):
                player1.CardsAnim.SetTrigger("Grapple");
                break;
            case ("Shove"):
                player1.CardsAnim.SetTrigger("Shove");
                break;
        }
    }

    private void Resolve(MoveCard P1Move, MoveCard P2Move)
    {
        Debug.Log(P1Move.cardName);
        Debug.Log(P2Move.cardName);
        timer = 10f;
        timerActive = false;

        // clashSentencePerformed = false;
         if(timer>=0 && encouragementSentences.Count>0)
             GlobalData.Instance.text.SetTextMessage(encouragementSentences[Random.Range(0, encouragementSentences.Count)]);

        player1.CardsAnim.SetTrigger("Reveal");
        player2.CardsAnim.SetTrigger("Reveal");

        if (P1Move.draws.Contains(P2Move))
        {
            timerActive = true;
            player2.FighterAnim.SetTrigger("Next");
            player1.FighterAnim.SetTrigger("Next");
            
            Debug.Log("DRAW");
            AudioManager.Instance.PlayCancelCard();
            AudioManager.Instance.PlayCrowdPanic(1f);
            GlobalData.Instance.Player1.ChosenMove = null;
            GlobalData.Instance.Player2.ChosenMove = null;
        }
        else if (P1Move.clashes.Contains(P2Move))
        {
            player2.FighterAnim.SetTrigger("Next");
            player1.FighterAnim.SetTrigger("Next");
            AudioManager.Instance.PlayCancelCard();
            AudioManager.Instance.PlayCrowdPanic(1f);
            ChooseMinigame();
            Debug.Log("Clash");
        }
        else if (P1Move.wins == P2Move)
        {
            timerActive = true;
            player2.FighterAnim.SetTrigger("Damage");
            player2.CharacterPrefab.GetComponent<FightersDataBinder>().GetHit(player2);
            AudioManager.Instance.UpdateCombatMusicByHealth(player1.Health, player2.Health);
            AudioManager.Instance.CheckLastHP(player1.Health, player2.Health);
            AudioManager.Instance.PlayCardSound(P1Move);
            AudioManager.Instance.PlayCardSound(P1Move);
            Debug.Log("PLAYER 1 WINS");
            AudioManager.Instance.PlayCrowdPanic(1f);
            GlobalData.Instance.Player1.ChosenMove = null;
            GlobalData.Instance.Player2.ChosenMove = null;
        }
        else if (P1Move.loses == P2Move)
        {
            timerActive = true;
            player1.FighterAnim.SetTrigger("Damage");
            player1.CharacterPrefab.GetComponent<FightersDataBinder>().GetHit(player1);
            AudioManager.Instance.UpdateCombatMusicByHealth(player1.Health, player2.Health);
            AudioManager.Instance.CheckLastHP(player1.Health, player2.Health);
            AudioManager.Instance.PlayCardSound(P2Move);
            AudioManager.Instance.PlayCrowdPanic(1f);
            AudioManager.Instance.PlayCardSound(P2Move);
            Debug.Log("PLAYER 1 LOSE");
            GlobalData.Instance.Player1.ChosenMove = null;
            GlobalData.Instance.Player2.ChosenMove = null;
        }
        //goToMinigame = true; //ELIMINA DOPO IL DEBUG 

        GlobalData.Instance.Player1.CardsAnim.SetTrigger("Out");
        GlobalData.Instance.Player2.CardsAnim.SetTrigger("Out");
    }
    public void OnFixedStateStay()
    {

    }

    public void OnStateExit()
    {
        player1.Input.gameObject.GetComponent<PlayerMoveInput>().enabled = false;
        if (!player2.IsCPUMode)
            player2.Input.gameObject.GetComponent<PlayerMoveInput>().enabled = false;

        // GlobalData.Instance.text.SetTextMessage(" ");

    }
    
    public void ChooseMinigame()
    {
        int index = Random.Range(0, 2);
        // int index = 1; // DEBUG: Sequence
        nextMinigame = index;
    }

    public void OnP1Received(MoveCard move)
    {
        if (timer > 7) return;
        GlobalData.Instance.Player1.ChosenMove = move;
        HandleAnimationsP1();
    }

    public void OnP2Received(MoveCard move)
    {
        if (timer > 7) return;
        GlobalData.Instance.Player2.ChosenMove = move;
        HandleAnimationsP2();
    }

    private MoveCard SelectRandomMoveCard()
    {
        int randomIndex = Random.Range(0, 4);
        return availableMoves[randomIndex];
    }
}
