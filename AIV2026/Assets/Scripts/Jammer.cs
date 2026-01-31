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

    public PlayerType PlayerType { get => playerType; set => playerType = value; }
    public CharacterType CharacterType { get => character; set => character = value; }
    public string Controller { get => controller; set => controller = value; }
    public PlayerInput Input { get => input; set => input = value; }
    public MoveCard ChosenMove { get => chosenMove; set => chosenMove = value; }
    public GameObject CharacterPrefab { get => characterPrefab; set => characterPrefab = value; }
   
    public int Health { get => health;}
    public bool IsCPUMode { get => isCPUMode; set => isCPUMode = value; }

    public void TakeAHit(int value = 1)
    {
        health -= value;
        if(health<=0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("dead");
        //logica
    }


}
