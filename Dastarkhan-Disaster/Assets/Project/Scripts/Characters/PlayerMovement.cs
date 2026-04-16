using UnityEngine;

namespace DastarkhanDisaster.Gameplay.Player
{
    /// <summary>
    /// Top-down planar movement on CharacterController.
    /// Reads input from PlayerInputHandler. Knows nothing about interactions.
    /// SOLID: Single Responsibility - only moves and rotates the body.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInputHandler))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Tuning")]
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _rotationSpeed = 12f;
        [SerializeField] private float _gravity = -20f;

        private CharacterController _controller;
        private PlayerInputHandler _input;
        private float _verticalVelocity;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<PlayerInputHandler>();
        }

        private void Update()
        {
            var raw = _input.MoveInput;
            var move = new Vector3(raw.x, 0f, raw.y);

            // Gravity (so CharacterController stays grounded on uneven floors later)
            if (_controller.isGrounded && _verticalVelocity < 0f) _verticalVelocity = -1f;
            _verticalVelocity += _gravity * Time.deltaTime;

            var velocity = move * _moveSpeed;
            velocity.y = _verticalVelocity;
            _controller.Move(velocity * Time.deltaTime);

            // Face movement direction
            if (move.sqrMagnitude > 0.01f)
            {
                var targetRot = Quaternion.LookRotation(move);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation, targetRot, _rotationSpeed * Time.deltaTime);
            }
        }
    }
}
