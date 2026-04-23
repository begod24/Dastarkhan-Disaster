using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _interactAction;
    private InputAction _dropAction;

    public Vector2 MoveInput { get; private set; }
    public bool InteractPressed { get; private set; }
    public bool InteractHeld { get; private set; }
    public bool DropPressed { get; private set; }

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
        _interactAction = _playerInput.actions["Interact"];
        _dropAction = _playerInput.actions["Drop"];
    }

    private void Update()
    {
        MoveInput = _moveAction.ReadValue<Vector2>();
        InteractPressed = _interactAction.WasPressedThisFrame();
        InteractHeld = _interactAction.IsPressed();
        DropPressed = _dropAction.WasPressedThisFrame();
    }
}
