using UnityEngine;

public class PlayerCarry : MonoBehaviour
{
    [SerializeField] private Transform _carryAnchor;
    [SerializeField] private int _playerId;

    public Ingredient CarriedItem { get; private set; }
    public bool IsCarrying => CarriedItem != null;
    public Transform CarryAnchor => _carryAnchor;

    public void Pickup(Ingredient item)
    {
        if (IsCarrying || item == null) return;

        CarriedItem = item;
        item.transform.SetParent(_carryAnchor);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        if (item.TryGetComponent<Collider>(out var col)) col.enabled = false;
        if (item.TryGetComponent<Rigidbody>(out var rb)) rb.isKinematic = true;

        EventBus.Raise(new PlayerCarryChangedEvent { PlayerId = _playerId, Carried = item });
    }

    public Ingredient Drop()
    {
        if (!IsCarrying) return null;

        var item = CarriedItem;
        CarriedItem = null;
        item.transform.SetParent(null);

        if (item.TryGetComponent<Collider>(out var col)) col.enabled = true;
        if (item.TryGetComponent<Rigidbody>(out var rb)) rb.isKinematic = false;

        EventBus.Raise(new PlayerCarryChangedEvent { PlayerId = _playerId, Carried = null });
        return item;
    }
}
