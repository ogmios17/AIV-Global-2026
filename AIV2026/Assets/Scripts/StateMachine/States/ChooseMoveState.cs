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

    public Jammer player1;
    public Jammer player2;
    public GameObject prefab;
    private GameObject prefabClone;
    private int nextMinigame;
    private bool chooseMinigame;
    private bool clashSentencePerformed;

    [Header("Available Move Cards")]
    public MoveCard[] availableMoves;
    private MoveCard p1Card;
    private MoveCard p2Card;
    public MoveCard P1Card { get { return p1Card; } }
    public MoveCard P2Card { get { return p2Card; } }


    public int NextMinigame { get => nextMinigame; }

    public void OnStateEnter()
    {
        p1Card = null;
        p2Card = null;
        timer = 8f;
        timerActive = true;
        nextMinigame = -1;
        chooseMinigame = false;
        clashSentencePerformed = false;

        player1 = GlobalData.Instance.Player1;
        player2 = GlobalData.Instance.Player2;

        Debug.Log(GlobalData.Instance.Player1);
        Debug.Log(GlobalData.Instance.Player2);

        prefabClone = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);

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
        if (player2.IsCPUMode && player2.ChosenMove == null)
        {
            int randomIndex = Random.Range(0, 4);
            player2.ChosenMove = availableMoves[randomIndex];
            
            // player2.ChosenMove = availableMoves[0]; // DEBUG: Attack
            Debug.Log($"CPU sceglie: {player2.ChosenMove.cardName}");
        }

        if (timerActive)
        {
            timer -= Time.deltaTime;
            switch (timer)
            {
                case <= 0:
                    GlobalData.Instance.text.SetTextMessage("Time's up!");
                    break;
                case <= 1:
                    GlobalData.Instance.text.SetTextMessage("1...");
                    break;
                case <= 2:
                    GlobalData.Instance.text.SetTextMessage("2...");
                    break;
                case <= 3:
                    if (chooseMinigame)
                    {
                        ChooseMinigame();
                    }
                    else
                        GlobalData.Instance.text.SetTextMessage("3...");
                    break;
                case <= 4:
                    if (chooseMinigame && clashSentences.Count > 0 && !clashSentencePerformed)
                    {
                        GlobalData.Instance.text.SetTextMessage(clashSentences[Random.Range(0, clashSentences.Count)]);
                        clashSentencePerformed = true;
                    }
                    else
                    {
                        GlobalData.Instance.Player1.ChosenMove = null;
                        GlobalData.Instance.Player2.ChosenMove = null;
                        GlobalData.Instance.text.SetTextMessage("Ready...");
                    }
                    break;

            }
        }
    }

    private void Resolve()
    {
        Debug.Log("Resolved");
        clashSentencePerformed = false;
        timerActive = false;
        if(timer>=0 && encouragementSentences.Count>0)
            GlobalData.Instance.text.SetTextMessage(encouragementSentences[Random.Range(0, encouragementSentences.Count)]);
        timer = 6f;


        player1.CardsAnim.SetTrigger("Reveal");
        player2.CardsAnim.SetTrigger("Reveal");

        if (p1Card.draws.Contains(p2Card))
        {
            timerActive = true;
            player2.FighterAnim.SetTrigger("Next");
            player1.FighterAnim.SetTrigger("Next");
            
            Debug.Log("DRAW");
            AudioManager.Instance.PlayCancelCard();
            AudioManager.Instance.PlayCrowdPanic(1f);
        }
        else if (p1Card.clashes.Contains(p2Card))
        {
            player2.FighterAnim.SetTrigger("Next");
            player1.FighterAnim.SetTrigger("Next");
            chooseMinigame = true;
            ChooseMinigame();
            Debug.Log("Clash");
            AudioManager.Instance.PlayCancelCard();
            AudioManager.Instance.PlayCrowdPanic(1f);
        }
        else if (p1Card.wins == p2Card)
        {
            timerActive = true;
            player2.FighterAnim.SetTrigger("Damage");
            player2.CharacterPrefab.GetComponent<FightersDataBinder>().GetHit(player2);
            AudioManager.Instance.UpdateCombatMusicByHealth(player1.Health, player2.Health);
            AudioManager.Instance.CheckLastHP(player1.Health, player2.Health);
            AudioManager.Instance.PlayCardSound(c1);
            AudioManager.Instance.PlayCardSound(p1Card);
            Debug.Log("PLAYER 1 WINS");
            AudioManager.Instance.PlayCrowdPanic(1f);
        }
        else if (p1Card.loses == p2Card)
        {
            timerActive = true;
            player1.FighterAnim.SetTrigger("Damage");
            player1.CharacterPrefab.GetComponent<FightersDataBinder>().GetHit(player1);
            AudioManager.Instance.UpdateCombatMusicByHealth(player1.Health, player2.Health);
            AudioManager.Instance.CheckLastHP(player1.Health, player2.Health);
            AudioManager.Instance.PlayCardSound(c2);
            AudioManager.Instance.PlayCrowdPanic(1f);
            AudioManager.Instance.PlayCardSound(p2Card);
            Debug.Log("PLAYER 1 LOSE");
        }
        //goToMinigame = true; //ELIMINA DOPO IL DEBUG 

        p1Card = null;
        p2Card = null;
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

        GlobalData.Instance.text.SetTextMessage(" ");

    }
    
    public void ChooseMinigame()
    {
        // int index = Random.Range(0, 1);
        int index = 0; // DEBUG: Mash
        nextMinigame = index;
    }

    public void OnP1Received(MoveCard move)
    {
        if (p1Card != null) return;
        p1Card = move;
        if(p2Card != null)
        {
            Resolve();
        }
    }

    public void OnP2Received(MoveCard move)
    {
        if (p2Card != null) return;
        p2Card = move;
        if (p1Card != null)
        {
            Resolve();
        }
    }
}
