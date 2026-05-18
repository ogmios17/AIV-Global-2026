using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSO", menuName = "Scriptable Objects/CharacterSO")]
public class CharacterSO : ScriptableObject
{
    public CharacterType characterType;
    public Sprite icon;
    public Sprite minigameMashIcon;
}
