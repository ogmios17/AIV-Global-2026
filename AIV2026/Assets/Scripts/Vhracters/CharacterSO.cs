using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSO", menuName = "Scriptable Objects/CharacterSO")]
public class CharacterSO : ScriptableObject
{
    public CharacterType characterType;
    public Sprite icon;
    public Sprite minigameMashIcon;
    public int abilityCost = 2;
    public int ultCost = 8;
    public string abilityText;
    public string ultiText;
}
