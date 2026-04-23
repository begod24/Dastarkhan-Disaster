using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _baseSpeed = 6f;
    [SerializeField] private float _rotationSmoothing = 12f;
    [SerializeField] private float _gravity = -20f;

    private CharacterController _controller;
    private PlayerInputHandler _input;
    private float _verticalVelocity;
    private float _speedMultiplier = 1f;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<PlayerInputHandler>();
    }

    public void SetSpeedMultiplier(float mult) => _speedMultiplier = mult;

    private void Update()
    {
        Vector2 input = _input.MoveInput;
        Vector3 direction = new Vector3(input.x, 0f, input.y);
        if (direction.sqrMagnitude > 1f) direction.Normalize();

        if (_controller.isGrounded && _verticalVelocity < 0f) _verticalVelocity = -2f;
        else _verticalVelocity += _gravity * Time.deltaTime;

        Vector3 move = direction * (_baseSpeed * _speedMultiplier);
        move.y = _verticalVelocity;
        _controller.Move(move * Time.deltaTime);

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion target = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * _rotationSmoothing);
        }
    }
}
