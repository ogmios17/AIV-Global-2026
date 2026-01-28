using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class CharacterSelectionManager : MonoBehaviour
{
    public GameObject characterCardPrefab;
    public List<CharacterSelectionCard> characterCards = new List<CharacterSelectionCard>();

    void Start()
    {
        foreach (CharacterSelectionCard characterCard in characterCards)
        {
            SpawnCharacterCard(characterCard);
        }
    }

    private void SpawnCharacterCard(CharacterSelectionCard characterCard)
    {
        GameObject cardObject = Instantiate(characterCardPrefab, transform);

        // Update UI elements
        Image characterImage = cardObject.transform.Find("CharacterImage").GetComponent<Image>();
        TextMeshProUGUI characterName = cardObject.transform.Find("CharacterNameContainer").GetComponentInChildren<TextMeshProUGUI>();

        characterImage.sprite = characterCard.characterImage;
        characterName.text = characterCard.characterName;

        // Changing image pivot
        Vector2 uiPivot = ConvertImagePivotToUIPivot(characterImage);
        characterImage.GetComponent<RectTransform>().pivot = uiPivot;

        // Apply zoom
        characterImage.GetComponent<RectTransform>().localScale *= characterCard.zoom;
    }

    private Vector2 ConvertImagePivotToUIPivot(Image spriteImage)
    {
        Vector2 pixelSize = new Vector2(spriteImage.sprite.texture.width, spriteImage.sprite.texture.height);
        Vector2 pixelPivot = spriteImage.sprite.pivot;
        Vector2 uiPivot = new Vector2(pixelPivot.x / pixelSize.x, pixelPivot.y / pixelSize.y);

        return uiPivot;
    }
}
