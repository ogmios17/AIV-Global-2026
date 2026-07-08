using System;
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
    [SerializeField] private SpriteRenderer p1SpritePlaceholder;
    [SerializeField] private SpriteRenderer p2SpritePlaceholder;
    [SerializeField] private SpriteRenderer p1barPlaceHolder;
    [SerializeField] private SpriteRenderer p2barPlaceHolder;
    private bool canPress = false;
    private float timer = 3f;

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
    private Queue<Direction> sequence1 = new Queue<Direction>();
    private Queue<Direction> sequence2 = new Queue<Direction>();
    private int player1SequenceIndex;
    private int player2SequenceIndex;

    // Game logic
    private bool isFinished = false;
    private bool isEnding = false; // Per evitare chiamate multiple a EndMinigame
    public bool IsFinished { get => isFinished; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        p1SpritePlaceholder.sprite = GlobalData.Instance.Characters[(int)GlobalData.Instance.Player1.CharacterType].icon;   
        p2SpritePlaceholder.sprite = GlobalData.Instance.Characters[(int)GlobalData.Instance.Player2.CharacterType].icon;
        p1barPlaceHolder.sprite = GlobalData.Instance.Characters[(int)GlobalData.Instance.Player1.CharacterType].minigameMashIcon;
        p2barPlaceHolder.sprite = GlobalData.Instance.Characters[(int)GlobalData.Instance.Player2.CharacterType].minigameMashIcon;

        player1 = GlobalData.Instance.Player1;
        player2 = GlobalData.Instance.Player2;

        player1SequenceIndex = 0;
        player2SequenceIndex = 0;

        // Switcho l'ActionMap
        player1.Input.SwitchCurrentActionMap(ActionMaps.Sequence);
        if (!player2.IsCPUMode)
            player2.Input.SwitchCurrentActionMap(ActionMaps.Sequence);

        InitSetup(player1, sequence1);
        InitSetup(player2, sequence2);
    }

    private void InitSetup(Jammer player, Queue<Direction> sequence)
    {
        // Player1 uses its own slots; everyone else (Player2 / CPU) uses the P2 slots.
        List<GameObject> slots = player.PlayerType == PlayerType.Player1 ? Player1Slots : Player2Slots;

        for (int i = 0; i < slots.Count; i++)
        {
            Direction dir = (Direction)UnityEngine.Random.Range(0, 4);
            sequence.Enqueue(dir);
            slots[i].GetComponent<SpriteRenderer>().sprite = DirectionToSprite(dir);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!canPress)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                canPress = true;
                GlobalData.Instance.text.SetTextMessage("");
            }else if(timer >= 2)
            {
                GlobalData.Instance.text.SetTextMessage("Ready...");
            }else if(timer >= 1)
            {
                GlobalData.Instance.text.SetTextMessage("Set...");
            }
            else
            {
                GlobalData.Instance.text.SetTextMessage("Go!");
            }
        }
        if (isFinished || isEnding) return;

        // CPU Logic
        if (canPress && player2.IsCPUMode)
        {
            cpuMashTimer += Time.deltaTime;
            if (cpuMashTimer >= cpuMashInterval)
            {
                if (sequence2.Count > 0)
                    ResolvePress(player2, player1, sequence2, Player2Slots, ref player2SequenceIndex,
                        AudioManager.Instance.PlaySpamButtonP2, sequence2.Peek());
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

        // Il perdente viene colpito; la UI vita si aggiorna via evento OnHealthChanged.
        globalLoser.TakeAHit();

        // Mana e segnali ability: 2 al vincitore, 1 al perdente (delta team).
        winner.CharacterPrefab.GetComponent<FightersDataBinder>().GainMana(2, winner);
        loser.CharacterPrefab.GetComponent<FightersDataBinder>().GainMana(1, loser);
        winner.onMoveHits?.Invoke();
        loser.onMoveMisses?.Invoke();
        if (globalLoser.FighterAnim != null)
            globalLoser.FighterAnim.SetTrigger(AnimTriggers.Damage);


        // Aspetta 3 secondi prima di segnalare la fine del minigioco
        StartCoroutine(WaitAndFinish());
    }

    private IEnumerator WaitAndFinish()
    {
        yield return new WaitForSeconds(3f);
        GlobalData.Instance.text.SetTextMessage("");
        isFinished = true;
    }

    public void Onp1Press(string pressedPath)
    {
        if (TryParseDirection(pressedPath, out Direction dir))
            ResolvePress(player1, player2, sequence1, Player1Slots, ref player1SequenceIndex,
                AudioManager.Instance.PlaySpamButtonP1, dir);
    }

    public void Onp2Press(string pressedPath)
    {
        if (TryParseDirection(pressedPath, out Direction dir))
            ResolvePress(player2, player1, sequence2, Player2Slots, ref player2SequenceIndex,
                AudioManager.Instance.PlaySpamButtonP2, dir);
    }

    /// <summary>
    /// Shared logic for a single input in the sequence minigame.
    /// On a correct press it advances the queue; on a wrong press the player loses.
    /// </summary>
    private void ResolvePress(Jammer self, Jammer other, Queue<Direction> sequence, List<GameObject> slots,
        ref int slotIndex, Action winSound, Direction pressed)
    {
        if (!canPress || sequence.Count == 0 || isFinished || isEnding) return;

        GameObject slot = slots[slotIndex];

        if (pressed == sequence.Peek())
        {
            sequence.Dequeue();
            slot.GetComponent<SpriteRenderer>().color = GreenColor;
            slot.GetComponentInParent<Animator>()?.SetTrigger(AnimTriggers.Right);
            slotIndex++;

            if (sequence.Count <= 0)
            {
                winSound();
                EndMinigame(self, other);
            }
        }
        else
        {
            slot.GetComponent<SpriteRenderer>().color = RedColor;
            slot.GetComponentInParent<Animator>()?.SetTrigger(AnimTriggers.Wrong);
            AudioManager.Instance.PlayUIError();
            EndMinigame(other, self);
        }
    }

    private Sprite DirectionToSprite(Direction dir) => dir switch
    {
        Direction.Up => upSprite,
        Direction.Down => downSprite,
        Direction.Right => rightSprite,
        _ => leftSprite,
    };

    /// <summary>Maps an InputSystem control path (dpad or keyboard) to a Direction.</summary>
    private bool TryParseDirection(string controlPath, out Direction dir)
    {
        string path = controlPath.ToLower();
        if (path.Contains("/up")) { dir = Direction.Up; return true; }
        if (path.Contains("/down")) { dir = Direction.Down; return true; }
        if (path.Contains("/right")) { dir = Direction.Right; return true; }
        if (path.Contains("/left")) { dir = Direction.Left; return true; }

        dir = Direction.Left;
        return false;
    }
}
