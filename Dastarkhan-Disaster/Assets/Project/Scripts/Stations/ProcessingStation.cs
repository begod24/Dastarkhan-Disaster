using UnityEngine;

public class ProcessingStation : StationBase
{
    [SerializeField] private StationConfigSO _config;
    [SerializeField] private Transform[] _slotAnchors;

    private Slot[] _slots;

    public StationConfigSO Config => _config;

    private class Slot
    {
        public Ingredient Item;
        public float Elapsed;
        public SlotState State;
    }

    private enum SlotState { Empty, Processing, Ready, Burned }

    private void Awake()
    {
        if (_config == null)
        {
            Debug.LogError($"[{name}] ProcessingStation has no StationConfigSO assigned.");
            enabled = false;
            return;
        }
        _slots = new Slot[_config.Capacity];
        for (int i = 0; i < _slots.Length; i++)
            _slots[i] = new Slot { State = SlotState.Empty };
    }

    public override string InteractionPrompt
    {
        get
        {
            if (_config == null) return Label;
            if (HasReadyItem()) return $"{Label}: Take";
            return $"{Label}: Place";
        }
    }

    public override bool CanInteract(PlayerController player)
    {
        if (_slots == null) return false;
        if (player.Carry.IsCarrying)
            return _config.AcceptsItem(player.Carry.CarriedItem) && FindEmptySlot() >= 0;
        return HasReadyItem();
    }

    public override void OnInteract(PlayerController player)
    {
        if (!player.Carry.IsCarrying)
        {
            if (TryTakeReadyItem(out var taken))
                player.Carry.Pickup(taken);
            return;
        }

        var held = player.Carry.CarriedItem;
        if (!_config.AcceptsItem(held)) return;

        int slotIndex = FindEmptySlot();
        if (slotIndex < 0) return;

        player.Carry.Drop();
        PlaceInSlot(slotIndex, held);
    }

    private int FindEmptySlot()
    {
        for (int i = 0; i < _slots.Length; i++)
            if (_slots[i].State == SlotState.Empty) return i;
        return -1;
    }

    private bool HasReadyItem()
    {
        for (int i = 0; i < _slots.Length; i++)
            if (_slots[i].State == SlotState.Ready || _slots[i].State == SlotState.Burned) return true;
        return false;
    }

    private bool TryTakeReadyItem(out Ingredient item)
    {
        item = null;
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].State == SlotState.Ready || _slots[i].State == SlotState.Burned)
            {
                item = _slots[i].Item;
                _slots[i].Item = null;
                _slots[i].Elapsed = 0f;
                _slots[i].State = SlotState.Empty;
                return true;
            }
        }
        return false;
    }

    private void PlaceInSlot(int index, Ingredient item)
    {
        var slot = _slots[index];
        slot.Item = item;
        slot.Elapsed = 0f;
        slot.State = SlotState.Processing;

        if (_slotAnchors != null && index < _slotAnchors.Length && _slotAnchors[index] != null)
        {
            item.transform.SetParent(_slotAnchors[index]);
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
        }

        if (item.TryGetComponent<Collider>(out var col)) col.enabled = false;
        if (item.TryGetComponent<Rigidbody>(out var rb)) rb.isKinematic = true;

        if (_config.ProcessDuration <= 0f) CompleteProcessing(index);
    }

    private void CompleteProcessing(int index)
    {
        var slot = _slots[index];
        slot.Item.SetState(_config.ProducesState);
        slot.State = SlotState.Ready;
        slot.Elapsed = 0f;
    }

    private void Update()
    {
        if (_slots == null) return;

        for (int i = 0; i < _slots.Length; i++)
        {
            var slot = _slots[i];
            if (slot.State == SlotState.Processing)
            {
                slot.Elapsed += Time.deltaTime;
                if (slot.Elapsed >= _config.ProcessDuration) CompleteProcessing(i);
            }
            else if (slot.State == SlotState.Ready && _config.CanBurn)
            {
                slot.Elapsed += Time.deltaTime;
                if (slot.Elapsed >= _config.BurnDelay)
                {
                    slot.Item.SetState(ProcessState.Burned);
                    slot.State = SlotState.Burned;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_slotAnchors == null) return;
        Gizmos.color = Color.cyan;
        foreach (var a in _slotAnchors)
            if (a != null) Gizmos.DrawWireSphere(a.position, 0.1f);
    }
}
