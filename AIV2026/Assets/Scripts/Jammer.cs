using UnityEngine;
using UnityEngine.InputSystem;

public class Jammer
{
    private PlayerType playerType;
    private Character character;
    private string controller;
    private MoveCard chosenMove;
    private PlayerInput input;

    public PlayerType PlayerType { get => playerType; set => playerType = value; }
    public Character Character { get => character; set => character = value; }
    public string Controller { get => controller; set => controller = value; }
    public PlayerInput Input { get => input; set => input = value; }
}
