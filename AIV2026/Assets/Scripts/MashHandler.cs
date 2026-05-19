using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MashHandler : MonoBehaviour
{
    private Jammer player1;
    private Jammer player2;
    public delegate void OnP1Mash();
    public delegate void OnP2Mash();
    public float mashStrength;
    private float points = 0;
    public GameObject targetLeft;
    public GameObject targetRight;
    public GameObject divider;
    [Range(0f, 0.1f)]
    public float modifierRange;
    [Tooltip("Chance will be 1 in x")]
    public int randomAdvantage_chance;
    public bool randomizeAdvantage = true;
    public float advantageStrength;

    [Header("CPU Settings")]
    public float cpuMashInterval  = 0.15f; // CPU masha ogni {cpuMashInterval} secondi
    private float cpuMashTimer;

    private bool isFinished = false;
    private bool isEnding = false; // Per evitare chiamate multiple a EndMinigame
    public bool IsFinished { get => isFinished; }
    private float timer =0;
    private List<string> loveSentences;
    private bool timerActive = true;
    private bool canPress = false;
    private float timerCountdown = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loveSentences = GlobalData.Instance.stateManager.MiniMashState.loveSentences;
        points = 0;
        player1 = GlobalData.Instance.Player1;
        player2 = GlobalData.Instance.Player2;

        player1.Input.SwitchCurrentActionMap("Mash");
        if (!player2.IsCPUMode)
            player2.Input.SwitchCurrentActionMap("Mash");
    }

    // Update is called once per frame
    void Update()
    {
        if (isFinished || isEnding) return;

        if (!canPress)
        {
            timerCountdown -= Time.deltaTime;
            if (timerCountdown <= 0)
            {
                canPress = true;
                GlobalData.Instance.text.SetTextMessage("");
            }
            else if (timerCountdown >= 2)
            {
                GlobalData.Instance.text.SetTextMessage("Ready...");
            }
            else if (timerCountdown >= 1)
            {
                GlobalData.Instance.text.SetTextMessage("Set...");
            }
            else
            {
                GlobalData.Instance.text.SetTextMessage("Go!");
            }
        }

        if (canPress && player2.IsCPUMode)
        {
            cpuMashTimer += Time.deltaTime;
            Debug.Log("cpuMashTimer: " + cpuMashTimer);

            if (cpuMashTimer >= cpuMashInterval)
            {
                Onp2Mash();
                cpuMashTimer = 0f;
            }
        }

        divider.transform.position = new Vector3(points,divider.transform.position.y,divider.transform.position.z);

        if (divider.transform.position.x <= targetLeft.transform.position.x)
        {
            EndMinigame(player2, player1);
        }
        if (divider.transform.position.x >= targetRight.transform.position.x)
        {
            EndMinigame(player1, player2);
        }

        timer += Time.deltaTime;
        if (timer >= 2 && timerActive)
        {
            GlobalData.Instance.text.SetTextMessage(loveSentences[Random.Range(0, loveSentences.Count)]);
            timer = 0;
        }
    }

    private void FixedUpdate()
    {
        if (isFinished || isEnding) return;

        if (canPress && randomizeAdvantage && Random.Range(0, randomAdvantage_chance) == 0) //p1 gets a help!
        {
            points -= advantageStrength;
        }
        if (canPress && randomizeAdvantage && Random.Range(0, randomAdvantage_chance) == 0) //p2 gets a help!
        {
            points += advantageStrength;
        }


    }

    public void Onp1Mash()
    {
        if (!canPress) return;
        points += mashStrength + Random.Range(0, modifierRange);

    }

    public void Onp2Mash()
    {
        if (!canPress) return;
        points -= mashStrength + Random.Range(0, modifierRange);
    }

    private void EndMinigame(Jammer winner, Jammer loser)
    {
        timerActive = false;
        if (isEnding) return; // Evita chiamate multiple
        isEnding = true;

        // Determina chi ha vinto/perso usando GlobalData per sicurezza
        bool player1Wins = (winner == player1);
        Jammer globalLoser = player1Wins ? GlobalData.Instance.Player2 : GlobalData.Instance.Player1;

        // Determina il nome del vincitore
        string winnerName;
        if (player1Wins)
            winnerName = "Player 1";
        else
            winnerName = GlobalData.Instance.Player2.IsCPUMode ? "CPU" : "Player 2";

        GlobalData.Instance.text.SetTextMessage($"{winnerName} Wins!");

        // Il perdente viene colpito (usa GlobalData per assicurarsi che la vita venga aggiornata)
        globalLoser.CharacterPrefab.GetComponent<FightersDataBinder>().GetHit(globalLoser);
        if (globalLoser.FighterAnim != null)
            globalLoser.FighterAnim.SetTrigger("Damage");

        Debug.Log($"{winnerName} wins the mash minigame! Loser health: {globalLoser.Health}");

        // Aspetta 3 secondi prima di segnalare la fine del minigioco
        StartCoroutine(WaitAndFinish());
    }

    private IEnumerator WaitAndFinish()
    {
        yield return new WaitForSeconds(3f);
        GlobalData.Instance.text.SetTextMessage("");
        isFinished = true;
    }
}
