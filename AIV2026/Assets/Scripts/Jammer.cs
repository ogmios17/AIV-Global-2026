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

    public PlayerType PlayerType { get => playerType; set => playerType = value; }
    public CharacterType CharacterType { get => character; set => character = value; }
    public string Controller { get => controller; set => controller = value; }
    public PlayerInput Input { get => input; set => input = value; }
    public MoveCard ChosenMove { get => chosenMove; set => chosenMove = value; }

}
