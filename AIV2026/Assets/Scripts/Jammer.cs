using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum CharacterType
{
    CrackKen,
    NotZilla
}

public class Jammer
{
    private PlayerType playerType;
    private CharacterType character;
    private InputDevice controller;
    private MoveCard chosenMove;
    private PlayerInput input;
    private GameObject characterPrefab;
    private readonly int maxHealth;
    private int health;
    private int mana = 0;
    private bool isCPUMode = false;
    private Animator fighterAnim;
    private Animator cardsAnim;
    private List<CardTypes> cards;

    /// <summary>Raised whenever the health changes. Args: (currentHealth, maxHealth).</summary>
    public event Action<int, int> OnHealthChanged;

    // Round-flow signals used by the character ability logic (CharacterBase subclasses).
    public Action onMoveChosen;
    public Action onMoveHits;
    public Action onMoveMisses;

    public Jammer(int maxHealth = 9)
    {
        this.maxHealth = maxHealth;
        health = maxHealth;
    }

    public PlayerType PlayerType { get => playerType; set => playerType = value; }

    public CharacterType CharacterType
    {
        get => character;
        set
        {
            // Re-assigning the same character must not create a second logic instance:
            // its constructor subscribes to the move events, and a duplicate would fire
            // every ability effect twice.
            if (CharacterLogic != null && character == value) return;

            character = value;
            CharacterLogic?.Unsubscribe();
            CharacterLogic = value switch
            {
                CharacterType.CrackKen => new CrackKen(this),
                CharacterType.NotZilla => new NotZilla(this),
                _ => CharacterLogic,
            };
        }
    }

    public CharacterBase CharacterLogic { get; private set; }

    public InputDevice Controller { get => controller; set => controller = value; }
    public PlayerInput Input { get => input; set => input = value; }
    public MoveCard ChosenMove { get => chosenMove; set => chosenMove = value; }
    public GameObject CharacterPrefab { get => characterPrefab; set => characterPrefab = value; }
    public Animator FighterAnim { get => fighterAnim; set => fighterAnim = value; }
    public Animator CardsAnim { get => cardsAnim; set => cardsAnim = value; }
    public List<CardTypes> Cards { get => cards; set => cards = value; }

    private bool isDead = false;
    public bool IsDead { get => isDead; }

    public int Health { get => health; }
    public int MaxHealth { get => maxHealth; }
    public int Mana { get => mana; }
    public bool IsCPUMode { get => isCPUMode; set => isCPUMode = value; }

    public void TakeAHit(int value = 1)
    {
        health = Mathf.Max(0, health - value);
        OnHealthChanged?.Invoke(health, maxHealth);

        if (health <= 0)
        {
            Die();
        }
    }

    public void Cure(int value = 1)
    {
        health = Mathf.Min(maxHealth, health + value);
        OnHealthChanged?.Invoke(health, maxHealth);
    }

    public bool SpendMana(int value)
    {
        if (value > mana) return false;

        mana -= value;
        return true;
    }

    public void GainMana(int value)
    {
        mana += value;
    }

    public void SetMana(int value)
    {
        mana = value;
    }

    public void Die()
    {
        // CPU jammers have no PlayerInput, and the prefab may not be wired yet: guard both.
        if (FighterAnim != null) FighterAnim.SetTrigger(AnimTriggers.Defeat);
        if (Input != null) Input.SwitchCurrentActionMap(ActionMaps.Defeat);

        GlobalData.Instance.text.SetTextMessage(CharacterFlavor.DefeatMessage(character));

        isDead = true;
    }
}
