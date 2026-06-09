using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour, IInteractable, ICarriable
{
    [SerializeField] private string _displayName = "Plate";
    [SerializeField] private Transform _stackAnchor;
    [SerializeField] private int _capacity = 4;
    [SerializeField] private float _stackSpacing = 0.06f;

    private readonly List<Ingredient> _contents = new();

    public Transform Transform => transform;
    public string DisplayName => _displayName;
    public IReadOnlyList<Ingredient> Contents => _contents;
    public bool IsEmpty => _contents.Count == 0;
    public bool IsFull => _contents.Count >= Mathf.Max(1, _capacity);

    public string InteractionPrompt => IsEmpty ? _displayName : $"{_displayName} ({_contents.Count})";

    public bool CanInteract(PlayerController player)
    {
        if (ReferenceEquals(player.Carry.Carried, this)) return false;
        if (player.Carry.IsCarrying)
            return player.Carry.CarriedIngredient != null && !IsFull;
        return true;
    }

    public void OnInteract(PlayerController player)
    {
        if (ReferenceEquals(player.Carry.Carried, this)) return;

        if (!player.Carry.IsCarrying)
        {
            player.Carry.Pickup(this);
            return;
        }

        var ingredient = player.Carry.CarriedIngredient;
        if (ingredient == null || IsFull) return;

        player.Carry.Drop();
        AddIngredient(ingredient);
    }

    public bool AddIngredient(Ingredient ingredient)
    {
        if (ingredient == null || IsFull) return false;

        _contents.Add(ingredient);

        var anchor = _stackAnchor != null ? _stackAnchor : transform;
        var t = ingredient.transform;
        t.SetParent(anchor);
        t.localPosition = Vector3.up * (_stackSpacing * (_contents.Count - 1));
        t.localRotation = Quaternion.identity;

        if (t.TryGetComponent<Collider>(out var col)) col.enabled = false;
        if (t.TryGetComponent<Rigidbody>(out var rb)) rb.isKinematic = true;

        EventBus.Raise(new PlateContentsChangedEvent { Plate = this });
        return true;
    }

    public void Clear()
    {
        for (int i = 0; i < _contents.Count; i++)
            if (_contents[i] != null) Destroy(_contents[i].gameObject);
        _contents.Clear();
        EventBus.Raise(new PlateContentsChangedEvent { Plate = this });
    }
}
