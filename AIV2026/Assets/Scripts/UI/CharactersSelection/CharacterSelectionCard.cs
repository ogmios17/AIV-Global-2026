using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CharacterSelectionCard", menuName = "UI/Character Selection Card", order = 1)]
public class CharacterSelectionCard : ScriptableObject
{
    public string characterName;
    public Sprite characterImage;
    public float zoom = 1.0f;

    [Header("Character")]
    public CharacterType characterType;
    [Tooltip("Wildcard card (e.g. \"Casual\"): picks a random playable character on submit.")]
    public bool isRandom;
    [Tooltip("Large preview shown when this card is hovered.")]
    public Sprite previewImage;
}
