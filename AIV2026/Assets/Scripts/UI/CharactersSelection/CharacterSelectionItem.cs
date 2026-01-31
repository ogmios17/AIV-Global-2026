using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;


/// <summary>
/// Handles visual feedback for character selection cards.
/// Supports dual-player simultaneous hovering with independent visual indicators.
/// </summary>
public class CharacterSelectionItem : MonoBehaviour
{
    // Player 1 visual elements (inner corners, bottom-right sprite)
    private Color _player1Color;
    private Sprite Player1Image;
    private GameObject _player1ImageObject;
    private Image _player1ImageComponent;
    private GameObject _boxCornerContainerP1;
    private Image[] _boxCornerImagesP1;

    // Player 2 visual elements (outer corners, bottom-left sprite)
    private Color _player2Color;
    private Sprite Player2Image;
    private GameObject _player2ImageObject;
    private Image _player2ImageComponent;
    private GameObject _boxCornerContainerP2;
    private Image[] _boxCornerImagesP2;

    // Hover state tracking
    private bool _isPlayer1Hovering = false;
    private bool _isPlayer2Hovering = false;

    void Start()
    {
        CharacterSelectionInputManager inputManager = transform.GetComponentInParent<CharacterSelectionInputManager>();
        _player1Color = inputManager.Player1Color;
        _player2Color = inputManager.Player2Color;
        Player1Image = inputManager.Player1Image;
        Player2Image = inputManager.IsCPUMode ? inputManager.CPUImage : inputManager.Player2Image;

        // Player 1 elements (existing structure)
        _player1ImageObject = transform.Find("PlayerImage")?.gameObject;
        if (_player1ImageObject != null)
        {
            _player1ImageComponent = _player1ImageObject.GetComponent<Image>();
            _player1ImageComponent.sprite = Player1Image;
        }

        _boxCornerContainerP1 = transform.Find("BoxQuadSelectorContainer")?.gameObject;
        if (_boxCornerContainerP1 != null)
        {
            _boxCornerImagesP1 = _boxCornerContainerP1.GetComponentsInChildren<Image>();
            SetCornerColors(_boxCornerImagesP1, _player1Color);
        }

        // Player 2 elements (new structure added in prefab)
        _player2ImageObject = transform.Find("PlayerImageP2")?.gameObject;
        if (_player2ImageObject != null)
        {
            _player2ImageComponent = _player2ImageObject.GetComponent<Image>();
            _player2ImageComponent.sprite = Player2Image;
        }

        _boxCornerContainerP2 = transform.Find("BoxQuadSelectorContainerP2")?.gameObject;
        if (_boxCornerContainerP2 != null)
        {
            _boxCornerImagesP2 = _boxCornerContainerP2.GetComponentsInChildren<Image>();
            SetCornerColors(_boxCornerImagesP2, _player2Color);
        }

        // Hide all elements initially
        HideAllVisuals();
    }

    /// <summary>
    /// Called by CharacterSelectionInputManager when a player hovers or leaves this card.
    /// </summary>
    public void SetPlayerHover(PlayerType player, bool isHovering)
    {
        if (player == PlayerType.Player1)
        {
            _isPlayer1Hovering = isHovering;
        }
        else if (player == PlayerType.Player2)
        {
            _isPlayer2Hovering = isHovering;
        }

        UpdateVisuals();
    }

    /// <summary>
    /// Updates all visual elements based on current hover states.
    /// </summary>
    private void UpdateVisuals()
    {
        // Player 1 visuals (inner corners, bottom-right sprite)
        if (_boxCornerContainerP1 != null)
            _boxCornerContainerP1.SetActive(_isPlayer1Hovering);
        
        if (_player1ImageObject != null)
            _player1ImageObject.SetActive(_isPlayer1Hovering);

        // Player 2 visuals (outer corners, bottom-left sprite)
        if (_boxCornerContainerP2 != null)
            _boxCornerContainerP2.SetActive(_isPlayer2Hovering);
        
        if (_player2ImageObject != null)
            _player2ImageObject.SetActive(_isPlayer2Hovering);
    }

    private void HideAllVisuals()
    {
        if (_boxCornerContainerP1 != null)
            _boxCornerContainerP1.SetActive(false);
        
        if (_player1ImageObject != null)
            _player1ImageObject.SetActive(false);
        
        if (_boxCornerContainerP2 != null)
            _boxCornerContainerP2.SetActive(false);
        
        if (_player2ImageObject != null)
            _player2ImageObject.SetActive(false);
    }

    private void SetCornerColors(Image[] corners, Color color)
    {
        if (corners == null) return;
        
        foreach (var corner in corners)
        {
            if (corner != null)
                corner.color = color;
        }
    }

    /// <summary>
    /// Returns true if this card is being hovered by the specified player.
    /// </summary>
    public bool IsHoveredBy(PlayerType player)
    {
        return player == PlayerType.Player1 ? _isPlayer1Hovering : _isPlayer2Hovering;
    }

    /// <summary>
    /// Returns true if this card is being hovered by any player.
    /// </summary>
    public bool IsHovered => _isPlayer1Hovering || _isPlayer2Hovering;
}
