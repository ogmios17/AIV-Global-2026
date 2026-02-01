using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SequenceHandler : MonoBehaviour
{

    [Header("UI")]
    [SerializeField] private Sprite upSprite;
    [SerializeField] private Sprite downSprite;
    [SerializeField] private Sprite leftSprite;
    [SerializeField] private Sprite rightSprite;

    [Header("Player 1 Slots")]
    [SerializeField] List<GameObject> Player1Slots;
    
    [Header("Player 2 Slots")]
    [SerializeField] List<GameObject> Player2Slots;

    // [New] CPU Settings
    [Header("CPU Settings")]
    public float cpuMashInterval = 0.5f;
    private float cpuMashTimer;

    private Jammer player1;
    private Jammer player2;

    private Color GreenColor = new(0.0509804f, 0.9803922f, 0.4078432f, 1);
    private Color RedColor = new(1.0f, 0.2352941f, 0.2745098f, 1);

    // Queues
    private Queue<string> sequence1 = new Queue<string>();
    private Queue<string> sequence2 = new Queue<string>();
    private int player1SequenceIndex;
    private int player2SequenceIndex;
    
    // Input Maps
    private string[] controllerInputs = {
        "/dpad/up",
        "/dpad/down",
        "/dpad/left",
        "/dpad/right",
    };
    private string[] keyboardInputs = {
        "/up",
        "/down",
        "/left",
        "/right",
    };

    // Game logic
    private bool isFinished = false;
    private bool isEnding = false; // Per evitare chiamate multiple a EndMinigame
    public bool IsFinished { get => isFinished; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player1 = GlobalData.Instance.Player1;
        player2 = GlobalData.Instance.Player2;

        player1SequenceIndex = 0;
        player2SequenceIndex = 0;

        // Switcho l'ActionMap
        player1.Input.SwitchCurrentActionMap("Sequence");
        if (!player2.IsCPUMode)
            player2.Input.SwitchCurrentActionMap("Sequence");

        // Setto la keyboard per il Player 1
        if (player1.Controller.Contains("Keyboard"))
            InitSetup(player1, keyboardInputs, sequence1);
        // Setto il controller per il Player 1
        else
            InitSetup(player1, controllerInputs, sequence1);

        // CPU
        if (player2.IsCPUMode)
        {
            InitSetup(player2, controllerInputs, sequence2);
        }
        else
        {
            // Setto la keyboard per il Player 2
            if (player2.Controller.Contains("Keyboard"))
                InitSetup(player2, keyboardInputs, sequence2);
            // Setto il controller per il Player 2
            else
                InitSetup(player2, controllerInputs, sequence2);
        }
    }

    private void InitSetup(Jammer player, string[] pool, Queue<string> sequence)
    {   
        // Per ogni indice da 0 a Player1Slots.Count (per velocita') inizializzo la coda e cambio la sprite nella UI
        for (int i = 0; i < Player1Slots.Count; i++)
        {
            var index = Random.Range(0, pool.Length);
            var item = pool[index];

            // Aggiungo nella coda
            sequence.Enqueue(item);

            // Cambio la sprite del giocatore 1
            if (player.PlayerType == PlayerType.Player1)
            {
                Player1Slots[i].GetComponent<SpriteRenderer>().sprite = InputToSprite(item);
            }
            // Cambio la sprite del giocatore 2
            else
            {
                Player2Slots[i].GetComponent<SpriteRenderer>().sprite = InputToSprite(item);
            }
            Debug.Log("Tasto: " + pool[index]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isFinished || isEnding) return;

        // CPU Logic
        if (player2.IsCPUMode)
        {
            cpuMashTimer += Time.deltaTime;
            if (cpuMashTimer >= cpuMashInterval)
            {
                if (sequence2.Count > 0)
                {
                    string nextExpected = sequence2.Peek();
                    Onp2Press(nextExpected);
                }
                cpuMashTimer = 0f;
            }
        }
    }

    private void EndMinigame(Jammer winner, Jammer loser)
    {
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

    public void Onp1Press(string pressed)
    {
        if (sequence1.Count == 0 || isFinished || isEnding) return;

        string expected = sequence1.Peek();

        // se pressed = dequeue allora tutto apposto se no muori
        if (pressed.ToLower().Contains(expected.ToLower()))
        {
            Debug.Log("Player1 Corretto!");
            sequence1.Dequeue();

            // Cambio il colore della sprite in verde
            Player1Slots[player1SequenceIndex].GetComponent<SpriteRenderer>().color = GreenColor;
            player1SequenceIndex++;

            // Check vittoria
            if (sequence1.Count <= 0)
            {
                AudioManager.Instance.PlaySpamButtonP1();
                EndMinigame(player1, player2);
            }
        }
        // Se sbaglio ho perso
        else
        {
            // Cambio il colore della sprite in rosso
            Player1Slots[player1SequenceIndex].GetComponent<SpriteRenderer>().color = RedColor;

            AudioManager.Instance.PlayUIError();
            EndMinigame(player2, player1);
        }
    }

    public void Onp2Press(string pressed)
    {
        if (sequence2.Count == 0 || isFinished || isEnding) return;

        string expected = sequence2.Peek();

        // se pressed = dequeue allora tutto apposto se no muori
        if (pressed.ToLower().Contains(expected.ToLower()))
        {
            Debug.Log("Player2 Corretto!");
            sequence2.Dequeue();

            // TODO Cambio il colore della sprite in verde
            Player2Slots[player2SequenceIndex].GetComponent<SpriteRenderer>().color = GreenColor;
            player2SequenceIndex++;

            // Check vittoria
            if (sequence2.Count <= 0)
            {
                AudioManager.Instance.PlaySpamButtonP2();
                EndMinigame(player2, player1);
            }
        }
        // Se sbaglio ho perso
        else
        {
            // Cambio il colore della sprite in rosso
            Player2Slots[player2SequenceIndex].GetComponent<SpriteRenderer>().color = RedColor;

            AudioManager.Instance.PlayUIError();
            EndMinigame(player1, player2);
        }
    }

    private Sprite InputToSprite(string input)
    {
        string parsedInput = ParseInput(input);

        return parsedInput switch
        {
            "up" => upSprite,
            "right" => rightSprite,
            "down" => downSprite,
            _ => leftSprite,
        };
    }

    private string ParseInput(string input)
    {
        if (input.Contains("/up")) return "up";
        else if (input.Contains("/right")) return "right";
        else if (input.Contains("/down")) return "down";
        else return "left";
    }
}
