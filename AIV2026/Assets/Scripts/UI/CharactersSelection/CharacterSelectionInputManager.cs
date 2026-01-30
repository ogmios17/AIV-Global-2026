using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

/// <summary>
/// Manages character selection input for two players simultaneously.
/// Each player can navigate the character grid independently with their gamepad.
/// </summary>
public class CharacterSelectionInputManager : MonoBehaviour
{
    [Header("Player Colors")]
    [SerializeField] private Color _player1Color = new Color(0.267f, 0.847f, 0.004f); // Green
    [SerializeField] private Color _player2Color = new Color(1f, 0.118f, 0.709f); // Pink

    [Header("Player Sprites")]
    [SerializeField] private Sprite _player1Image;
    [SerializeField] private Sprite _player2Image;

    [Header("Player Characters GameObjects")]
    [SerializeField] private GameObject _player1CharacterGameObject;
    [SerializeField] private GameObject _player2CharacterGameObject;

    [Header("Player Characters Images")]
    [SerializeField] private Sprite _notzillaCharacterImage;
    [SerializeField] private Sprite _crackkenCharacterImage;
    [SerializeField] private Sprite _casualCharacterImage;

    [Header("Navigation Settings")]
    [SerializeField] private float _navigationCooldown = 0.2f;

    // References to all character selection items
    private List<CharacterSelectionItem> _characterItems = new List<CharacterSelectionItem>();
    
    // Current selection index for each player
    private int _player1Index = 0;
    private int _player2Index = 1;
    
    // Navigation cooldown timers
    private float _player1CooldownTimer = 0f;
    private float _player2CooldownTimer = 0f;
    
    // Input actions for each player - Navigation
    private InputAction _player1NavigateAction;
    private InputAction _player2NavigateAction;
    
    // Input actions for each player - Submit (X button / South button)
    private InputAction _player1SubmitAction;
    private InputAction _player2SubmitAction;
    
    // Track previous navigation values to detect new inputs
    private Vector2 _player1PrevNav = Vector2.zero;
    private Vector2 _player2PrevNav = Vector2.zero;

    // Events for when a player confirms their selection
    public event Action<PlayerType, int> OnPlayerConfirmSelection;

    public Color Player1Color => _player1Color;
    public Color Player2Color => _player2Color;

    public Sprite Player1Image => _player1Image;
    public Sprite Player2Image => _player2Image;

    void Start()
    {
        // Get all character items from the selection manager
        CharacterSelectionManager selectionManager = GetComponent<CharacterSelectionManager>();
        if (selectionManager != null)
        {
            // Get all CharacterSelectionItem components from children
            _characterItems = new List<CharacterSelectionItem>(GetComponentsInChildren<CharacterSelectionItem>());
        }

        SetupInputActions();
        
        // Initialize - set P1 and P2 to first items
        if (_characterItems.Count > 0)
        {
            UpdatePlayerSelection(PlayerType.Player1, _player1Index, -1);
            UpdatePlayerSelection(PlayerType.Player2, _player2Index, -1);
        }
    }

    private void SetupInputActions()
    {
        // Get all connected gamepads
        var gamepads = Gamepad.all;
        
        Debug.Log($"[CharacterSelectionInputManager] Found {gamepads.Count} gamepads");
        
        // === PLAYER 1 SETUP ===
        _player1NavigateAction = new InputAction("P1_Navigate", InputActionType.Value);
        _player1SubmitAction = new InputAction("P1_Submit", InputActionType.Button);
        
        if (gamepads.Count >= 1)
        {
            // Player 1 uses FIRST gamepad specifically
            string pad1Path = gamepads[0].path;
            Debug.Log($"[CharacterSelectionInputManager] P1 using gamepad: {pad1Path}");
            
            _player1NavigateAction.AddBinding($"{pad1Path}/leftStick");
            _player1NavigateAction.AddBinding($"{pad1Path}/dpad");
            
            // Submit button (X on PlayStation / A on Xbox - buttonSouth)
            _player1SubmitAction.AddBinding($"{pad1Path}/buttonSouth");
        }
        
        // Also allow keyboard for P1 (WASD + Space)
        _player1NavigateAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
        _player1SubmitAction.AddBinding("<Keyboard>/space");
        
        _player1NavigateAction.Enable();
        _player1SubmitAction.Enable();
        _player1SubmitAction.performed += ctx => OnSubmitPressed(PlayerType.Player1);

        // === PLAYER 2 SETUP ===
        _player2NavigateAction = new InputAction("P2_Navigate", InputActionType.Value);
        _player2SubmitAction = new InputAction("P2_Submit", InputActionType.Button);
        
        if (gamepads.Count >= 2)
        {
            // Player 2 uses SECOND gamepad specifically
            string pad2Path = gamepads[1].path;
            Debug.Log($"[CharacterSelectionInputManager] P2 using gamepad: {pad2Path}");
            
            _player2NavigateAction.AddBinding($"{pad2Path}/leftStick");
            _player2NavigateAction.AddBinding($"{pad2Path}/dpad");
            
            // Submit button
            _player2SubmitAction.AddBinding($"{pad2Path}/buttonSouth");
        }
        else
        {
            Debug.Log("[CharacterSelectionInputManager] P2 fallback to arrow keys");
        }
        
        // Also allow keyboard for P2 (Arrows + Enter)
        _player2NavigateAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/rightArrow");
        _player2SubmitAction.AddBinding("<Keyboard>/enter");
        
        _player2NavigateAction.Enable();
        _player2SubmitAction.Enable();
        _player2SubmitAction.performed += ctx => OnSubmitPressed(PlayerType.Player2);
    }

    private void OnSubmitPressed(PlayerType player)
    {
        int selectedIndex = player == PlayerType.Player1 ? _player1Index : _player2Index;
        
        Debug.Log($"[CharacterSelectionInputManager] {player} confirmed selection at index {selectedIndex}");
        
        // Invoke event for external listeners
        OnPlayerConfirmSelection?.Invoke(player, selectedIndex);
        
        // Also click the button on the card if it exists
        if (selectedIndex >= 0 && selectedIndex < _characterItems.Count)
        {
            Button button = _characterItems[selectedIndex].GetComponent<Button>();
            if (button != null)
            {
                button.onClick.Invoke();
            }
        }
    }

    void Update()
    {
        if (_characterItems.Count == 0) return;

        // Update cooldown timers
        _player1CooldownTimer -= Time.deltaTime;
        _player2CooldownTimer -= Time.deltaTime;

        // Handle Player 1 navigation
        if (_player1NavigateAction != null)
        {
            Vector2 nav1 = _player1NavigateAction.ReadValue<Vector2>();
            HandleNavigation(PlayerType.Player1, nav1, ref _player1PrevNav, ref _player1Index, ref _player1CooldownTimer);
        }

        // Handle Player 2 navigation
        if (_player2NavigateAction != null)
        {
            Vector2 nav2 = _player2NavigateAction.ReadValue<Vector2>();
            HandleNavigation(PlayerType.Player2, nav2, ref _player2PrevNav, ref _player2Index, ref _player2CooldownTimer);
        }
    }

    private void HandleNavigation(PlayerType player, Vector2 currentNav, ref Vector2 prevNav, ref int currentIndex, ref float cooldownTimer)
    {
        // Only process new navigation input when cooldown has expired
        if (cooldownTimer > 0) 
        {
            prevNav = currentNav;
            return;
        }

        int newIndex = currentIndex;
        bool navigated = false;

        // Horizontal navigation (left/right through the list)
        if (currentNav.x > 0.5f && prevNav.x <= 0.5f)
        {
            newIndex = (currentIndex + 1) % _characterItems.Count;
            navigated = true;
        }
        else if (currentNav.x < -0.5f && prevNav.x >= -0.5f)
        {
            newIndex = (currentIndex - 1 + _characterItems.Count) % _characterItems.Count;
            navigated = true;
        }

        if (navigated)
        {
            int oldIndex = currentIndex;
            currentIndex = newIndex;
            cooldownTimer = _navigationCooldown;
            UpdatePlayerSelection(player, newIndex, oldIndex);
        }

        prevNav = currentNav;
    }

    private void UpdatePlayerSelection(PlayerType player, int newIndex, int oldIndex)
    {
        // Remove hover from old item
        if (oldIndex >= 0 && oldIndex < _characterItems.Count)
        {
            _characterItems[oldIndex].SetPlayerHover(player, false);
        }

        // Add hover to new item
        if (newIndex >= 0 && newIndex < _characterItems.Count)
        {
            _characterItems[newIndex].SetPlayerHover(player, true);
            UpdateCharacterImage(player, _characterItems[newIndex]);
        }
    }

    void OnDestroy()
    {
        if (_player1SubmitAction != null)
        {
            _player1SubmitAction.performed -= ctx => OnSubmitPressed(PlayerType.Player1);
        }
        if (_player2SubmitAction != null)
        {
            _player2SubmitAction.performed -= ctx => OnSubmitPressed(PlayerType.Player2);
        }
        
        _player1NavigateAction?.Disable();
        _player1NavigateAction?.Dispose();
        _player2NavigateAction?.Disable();
        _player2NavigateAction?.Dispose();
        _player1SubmitAction?.Disable();
        _player1SubmitAction?.Dispose();
        _player2SubmitAction?.Disable();
        _player2SubmitAction?.Dispose();
    }

    private void UpdateCharacterImage(PlayerType player, CharacterSelectionItem characterItem)
    {
        Sprite sprite = GetCharacterByCharacterItem(characterItem);
        Debug.Log(sprite.name);

        switch (player)
        {
            case PlayerType.Player1:
                // Prendo la CharacterImage del Player1 (_player1CharacterGameObject.sprite)

                // TODO Leggero slide verso sinistra

                // Sostituisco l'immagine
                //TODO Leggero slide verso sinistra
                _player1CharacterGameObject.transform.GetComponent<Image>().sprite = sprite;
                break;
            case PlayerType.Player2:
                _player2CharacterGameObject.transform.GetComponent<Image>().sprite = sprite;
                break;
            case PlayerType.CPU:
                _player2CharacterGameObject.transform.GetComponent<Image>().sprite = sprite;
                break;
            default:
                Debug.LogWarning($"[CharacterSelectionInputManager] Unknown character item name: {characterItem.name}");
                break;
        }
    }

    private Sprite GetCharacterByCharacterItem(CharacterSelectionItem characterItem)
    {
        string name = characterItem.name;

        switch(name)
        {
            case "Notzilla":
                return _notzillaCharacterImage;
            case "Crack-Ken":
                return _crackkenCharacterImage;
            case "Casual":
                return _casualCharacterImage;
            default:
                return _notzillaCharacterImage;
        }
    }
}
