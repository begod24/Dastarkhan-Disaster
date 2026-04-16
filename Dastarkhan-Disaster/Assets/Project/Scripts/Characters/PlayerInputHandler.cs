using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DastarkhanDisaster.Gameplay.Player
{
    /// <summary>
    /// Wraps Unity's PlayerInput component. All other player scripts read from this,
    /// never directly from InputSystem. Keeps input swap-able (keyboard, gamepad, AI).
    /// SOLID: Single Responsibility - only translates raw input to public API.
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputHandler : MonoBehaviour
    {
        public Vector2 MoveInput { get; private set; }
        public event Action OnInteractPressed;
        public event Action OnDropPressed;

        private PlayerInput _playerInput;
        private InputAction _moveAction;
        private InputAction _interactAction;
        private InputAction _dropAction;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _moveAction = _playerInput.actions["Move"];
            _interactAction = _playerInput.actions["Interact"];
            _dropAction = _playerInput.actions["Drop"];
        }

        private void OnEnable()
        {
            _interactAction.performed += HandleInteract;
            _dropAction.performed += HandleDrop;
        }

        private void OnDisable()
        {
            _interactAction.performed -= HandleInteract;
            _dropAction.performed -= HandleDrop;
        }

        private void Update() => MoveInput = _moveAction.ReadValue<Vector2>();

        private void HandleInteract(InputAction.CallbackContext _) => OnInteractPressed?.Invoke();
        private void HandleDrop(InputAction.CallbackContext _) => OnDropPressed?.Invoke();
    }
}
