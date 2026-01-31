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
    [Header("Game Mode")]
    [SerializeField] private bool _isCPUMode;
    public bool IsCPUMode { get => _isCPUMode; set => _isCPUMode = value; }

    [Header("Player Colors")]
    [SerializeField] private Color _player1Color = new Color(0.267f, 0.847f, 0.004f); // Green
    [SerializeField] private Color _player2Color = new Color(1f, 0.118f, 0.709f); // Pink

    [Header("Player Sprites")]
    [SerializeField] private Sprite _player1Image;
    [SerializeField] private Sprite _player2Image;
    [SerializeField] private Sprite _cpuImage;

    [Header("Player Characters GameObjects")]
    [SerializeField] private GameObject _player1CharacterGameObject;
    [SerializeField] private GameObject _player2CharacterGameObject;

    [Header("Player Characters Images")]
    [SerializeField] private Sprite _notzillaCharacterImage;
    [SerializeField] private Sprite _crackkenCharacterImage;
    [SerializeField] private Sprite _casualCharacterImage;

    [Header("Navigation Settings")]
    [SerializeField] private float _navigationCooldown = 0.2f;

    [Header("Player Prefab")]
    [SerializeField] private GameObject Player1Prefab;

    [Header("Player Input Managers")]
    [SerializeField] private GameObject OldPlayerInputManager;


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

    public Color Player1Color => _player1Color;
    public Color Player2Color => _player2Color;

    public Sprite Player1Image => _player1Image;
    public Sprite Player2Image => _player2Image;
    public Sprite CPUImage => _cpuImage;

    private CharacterType? _player1CharacterChoice ;
    private CharacterType? _player2CharacterChoice ;

    public bool _choicesDone = false;

    GameObject player1PrefabInstance;
    GameObject player2PrefabInstance;

    void Start()
    {
        Debug.Log("Is CPU mode: " + _isCPUMode);

        var gamepads = Gamepad.all;

        // Get all character items from the selection manager
        CharacterSelectionManager selectionManager = GetComponent<CharacterSelectionManager>();
        if (selectionManager != null)
        {
            // Get all CharacterSelectionItem components from children
            _characterItems = new List<CharacterSelectionItem>(GetComponentsInChildren<CharacterSelectionItem>());
        }

        // Human P1
        player1PrefabInstance = Instantiate(Player1Prefab);
        
        // CPU P2
        if(_isCPUMode)
        {
            // Auto-seleziona un personaggio random per la CPU
            AutoSelectCPUCharacter();
        }
        // Human P2
        else
        {
            player2PrefabInstance = Instantiate(Player1Prefab); // Human P2
        }

        // Initialize - set P1 and P2 to first items
        if (_characterItems.Count > 0)
        {
            UpdatePlayerSelection(PlayerType.Player1, _player1Index, -1);
            UpdatePlayerSelection(PlayerType.Player2, _player2Index, -1);
        }
    }

    public void OnSubmitPressed(PlayerType player)
    {
        if(player == PlayerType.Player1 && _player1CharacterChoice != null) return;
        if(player == PlayerType.Player2 && _player2CharacterChoice != null) return;
        
        int selectedIndex = player == PlayerType.Player1 ? _player1Index : _player2Index;
        CharacterType choice = GetCharacterByIndex(selectedIndex);

        if(player == PlayerType.Player1)
        {
            _player1CharacterChoice = choice;

            // In modalità CPU, la scelta del P2 è già fatta
            if (_isCPUMode && _player2CharacterChoice != null)
            {
                FinalizeSelection();
            }
        }
        else if(!_isCPUMode) // P2 può scegliere solo se NON è CPU mode
        {
            _player2CharacterChoice = choice;
        }

        // Aggiorno _choicesDone
        if (_player1CharacterChoice != null && _player2CharacterChoice != null)
        {
            FinalizeSelection();
        }

    }

    void Update()
    {
        if (_characterItems.Count == 0) return;

        // Update cooldown timers
        _player1CooldownTimer -= Time.deltaTime;
        _player2CooldownTimer -= Time.deltaTime;
    }

    public void HandleNavigationP1(Vector2 nav1)
    {
        // Se è stato già scelto un personaggio per il P1, ignora l'input
        if (_player1CharacterChoice != null) return;

        Debug.Log("Handle navigation p1");
        // Only process new navigation input when cooldown has expired
        if (_player1CooldownTimer > 0) 
        {
            _player1PrevNav = nav1;
            return;
        }

        int newIndex = _player1Index;
        bool navigated = false;

        //Horizontal navigation(left/ right through the list)
        if (nav1.x > 0.5f && _player1PrevNav.x <= 0.5f)
        {
            newIndex = (_player1Index + 1) % _characterItems.Count;
            navigated = true;
        }
        else if (nav1.x < -0.5f && _player1PrevNav.x >= -0.5f)
        {
            newIndex = (_player1Index - 1 + _characterItems.Count) % _characterItems.Count;
            navigated = true;
        }

        if (navigated)
        {
            int oldIndex = _player1Index;
            _player1Index = newIndex;
            _player1CooldownTimer = _navigationCooldown;
            UpdatePlayerSelection(PlayerType.Player1, newIndex, oldIndex);
        }

        _player1PrevNav = nav1;
    }

    public void HandleNavigationP2(Vector2 nav2)
    {
        // Se è CPU mode, ignora l'input del P2
        if (_isCPUMode) return;

        // Se è stato già scelto un personaggio per il P2, ignora l'input
        if (_player2CharacterChoice != null) return;

        Debug.Log("Handle navigation p2");
        // Only process new navigation input when cooldown has expired
        if (_player2CooldownTimer > 0)
        {
            _player2PrevNav = nav2;
            return;
        }

        int newIndex = _player2Index;
        bool navigated = false;

        // Horizontal navigation (left/right through the list)
        if (nav2.x > 0.5f && _player2PrevNav.x <= 0.5f)
        {
            newIndex = (_player2Index + 1) % _characterItems.Count;
            navigated = true;
        }
        else if (nav2.x < -0.5f && _player2PrevNav.x >= -0.5f)
        {
            newIndex = (_player2Index - 1 + _characterItems.Count) % _characterItems.Count;
            navigated = true;
        }

        if (navigated)
        {
            int oldIndex = _player2Index;
            _player2Index = newIndex;
            _player2CooldownTimer = _navigationCooldown;
            UpdatePlayerSelection(PlayerType.Player2, newIndex, oldIndex);
        }

        _player2PrevNav = nav2;
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

    private void UpdateCharacterImage(PlayerType player, CharacterSelectionItem characterItem)
    {
        Sprite sprite = GetCharacterByCharacterItem(characterItem);

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

    private CharacterType GetCharacterByIndex(int index)
    {
        switch(index)
        {
            case 0:
                return CharacterType.NotZilla;
            case 1:
                return CharacterType.CrackKen;
            case 2:
                int randomInt = UnityEngine.Random.Range(0, 2);
                return randomInt == 0 ? CharacterType.NotZilla : CharacterType.CrackKen;
            default:
                return CharacterType.NotZilla;
        }
    }

    private void DisablePlayer1Input()
    {
        if (_player1SubmitAction != null)
        {
            _player1SubmitAction.performed -= ctx => OnSubmitPressed(PlayerType.Player1);
        }
        _player1NavigateAction?.Disable();
        _player1SubmitAction?.Disable();
        _player1NavigateAction?.Dispose();
        _player1SubmitAction?.Dispose();
    }

    private void DisablePlayer2Input()
    {
        if (_player2SubmitAction != null)
        {
            _player2SubmitAction.performed -= ctx => OnSubmitPressed(PlayerType.Player2);
        }
        _player2NavigateAction?.Disable();
        _player2NavigateAction?.Dispose();
        _player2SubmitAction?.Disable();
        _player2SubmitAction?.Dispose();
    }

    private void AutoSelectCPUCharacter()
    {
        // Seleziona random tra i personaggi disponibili
        int randomIndex = UnityEngine.Random.Range(0, _characterItems.Count);
        _player2Index = randomIndex;
        _player2CharacterChoice = GetCharacterByIndex(randomIndex);

        // Aggiorna la UI
        UpdatePlayerSelection(PlayerType.CPU, randomIndex, -1);
    }

    private void FinalizeSelection()
    {
        _choicesDone = true;

        // Player 1 - sempre umano
        player1PrefabInstance.GetComponent<PlayerBinder>().Character = _player1CharacterChoice.Value;

        if(_isCPUMode)
        {
            // Crea Jammer CPU manualmente (senza PlayerInput)
            Jammer cpuJammer = new Jammer();
            cpuJammer.PlayerType = PlayerType.CPU;
            cpuJammer.CharacterType = _player2CharacterChoice.Value;
            cpuJammer.IsCPUMode = true;
            cpuJammer.Input = null; // CPU non ha input
        
            GlobalData.Instance.Player2 = cpuJammer;
        }
        else
        {
            player1PrefabInstance.GetComponent<PlayerBinder>().Character = _player2CharacterChoice.Value;
        }

        SceneLoader.Instance.Load("SampleScene");
    }
}
