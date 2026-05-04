using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerCarry : MonoBehaviour
{
    [SerializeField] private Transform _carryAnchor;
    [SerializeField] private int _playerId;
    [SerializeField] private float _dropExtraDistance = 0.4f;
    [SerializeField] private float _dropUpOffset = 0.15f;

    private PlayerInputHandler _input;
    private CharacterController _controller;

    public Ingredient CarriedItem { get; private set; }
    public bool IsCarrying => CarriedItem != null;
    public Transform CarryAnchor => _carryAnchor;

    private void Awake()
    {
        _input = GetComponent<PlayerInputHandler>();
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (Time.timeScale <= 0f) return;
        if (_input != null && _input.DropPressed && IsCarrying) DropToGround();
    }

    public void Pickup(Ingredient item)
    {
        if (IsCarrying || item == null) return;

        CarriedItem = item;
        item.transform.SetParent(_carryAnchor);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        if (item.TryGetComponent<Rigidbody>(out var rb)) rb.isKinematic = true;
        if (item.TryGetComponent<Collider>(out var col)) col.enabled = false;

        EventBus.Raise(new PlayerCarryChangedEvent { PlayerId = _playerId, Carried = item });
    }

    public Ingredient Drop()
    {
        if (!IsCarrying) return null;

        var item = CarriedItem;
        CarriedItem = null;
        item.transform.SetParent(null);

        if (item.TryGetComponent<Collider>(out var col)) col.enabled = true;
        if (item.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        EventBus.Raise(new PlayerCarryChangedEvent { PlayerId = _playerId, Carried = null });
        return item;
    }

    public void DropToGround()
    {
        var item = Drop();
        if (item == null) return;

        float playerRadius = _controller != null ? _controller.radius : 0.5f;
        float itemRadius = 0.25f;
        if (item.TryGetComponent<Collider>(out var itemCol))
            itemRadius = itemCol.bounds.extents.magnitude;

        float forwardDist = playerRadius + itemRadius + _dropExtraDistance;
        Vector3 dropPos = transform.position + transform.forward * forwardDist + Vector3.up * _dropUpOffset;
        item.transform.position = dropPos;
    }
}