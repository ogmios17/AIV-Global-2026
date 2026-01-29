using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public Vector2 NavigationInput { get; set; }

    private InputAction _navigationAction;

    public static PlayerInput PlayerInput { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        PlayerInput = GetComponent<PlayerInput>();
        _navigationAction = PlayerInput.actions["Navigate"];
    }

    private void Update()
    {
        NavigationInput = _navigationAction.ReadValue<Vector2>();
    }
}
