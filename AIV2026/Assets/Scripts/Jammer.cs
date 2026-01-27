using UnityEngine;

public class Jammer
{
    private PlayerType playerType;
    private Character character;
    private string controller;
    private MoveCard chosenMove;

    public PlayerType PlayerType { get => playerType; set => playerType = value; }
    public Character Character { get => character; set => character = value; }
    public string Controller { get => controller; set => controller = value; }
    public MoveCard ChosenMove { get => chosenMove; set => chosenMove = value; }
}
