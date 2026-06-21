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
    private string controller;
    private MoveCard chosenMove;
    private PlayerInput input;
    private GameObject characterPrefab;
    private int health = 9;
    private bool isCPUMode = false;
    private Animator fighterAnim;
    private Animator cardsAnim;
    private List<CardTypes> cards;
    private int mana = 0;
    public Action onMoveChosen;
    public Action onMoveHits;
    public Action onMoveMisses;

    public PlayerType PlayerType { get => playerType; set => playerType = value; }
    public CharacterType CharacterType { get => character; set => character = value; }
    public string Controller { get => controller; set => controller = value; }
    public PlayerInput Input { get => input; set => input = value; }
    public MoveCard ChosenMove { get => chosenMove; set => chosenMove = value; }
    public GameObject CharacterPrefab { get => characterPrefab; set => characterPrefab = value; }
    public Animator FighterAnim { get => fighterAnim; set => fighterAnim = value; }
    public Animator CardsAnim { get => cardsAnim; set => cardsAnim = value; }
    public List<CardTypes> Cards  { get => cards; set => cards = value; }

    public ICharacter characterMethods;


    private bool isDead = false;
    public bool IsDead { get => isDead; }


    public int Health { get => health;}
    public bool IsCPUMode { get => isCPUMode; set => isCPUMode = value; }
    public int Mana { get => mana; set => mana = value; }

    public void TakeAHit(int value = 1)
    {
        health -= value;
        if(health<=0)
        {
            Die();
        }
    }

    public void Cure(int value = 1)
    {
        health += value;
    }

    public bool SpendMana(int value)
    {
        if(value > mana)
        {
            return false;
        }
        mana -= value;
        return true;
    }

    public void GainMana(int value)
    {
        mana+=value;
    }

    public void SetMana(int value)
    {
        mana = value;
    }
    public void Die()
    {
        Debug.Log("dead");
        Debug.Log("abcde " + character);
        FighterAnim.SetTrigger("Defeat");
        Input.SwitchCurrentActionMap("Defeat");
        if(character == CharacterType.NotZilla)
        {
            GlobalData.Instance.text.SetTextMessage("Not Zilla was Godzilla all along!");
        }else if(character == CharacterType.CrackKen)
        {
            GlobalData.Instance.text.SetTextMessage("Krack Ken was a squid all along!");
        }
        //logica

        isDead = true;
    }

}
