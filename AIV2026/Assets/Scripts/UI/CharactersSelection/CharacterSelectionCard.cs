using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CharacterSelectionCard", menuName = "UI/Character Selection Card", order = 1)]
public class CharacterSelectionCard : ScriptableObject
{
    public string characterName;
    public Sprite characterImage;
    public float zoom = 1.0f;
}
