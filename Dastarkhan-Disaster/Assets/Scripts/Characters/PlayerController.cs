using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerCarry))]
[RequireComponent(typeof(PlayerInteraction))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private int _playerId;

    public int PlayerId => _playerId;
    public PlayerInputHandler Input { get; private set; }
    public PlayerMovement Movement { get; private set; }
    public PlayerCarry Carry { get; private set; }
    public PlayerInteraction Interaction { get; private set; }

    private void Awake()
    {
        Input = GetComponent<PlayerInputHandler>();
        Movement = GetComponent<PlayerMovement>();
        Carry = GetComponent<PlayerCarry>();
        Interaction = GetComponent<PlayerInteraction>();
    }
}
