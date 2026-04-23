using UnityEngine;
using TMPro;

public class DebugHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _heldText;
    [SerializeField] private TextMeshProUGUI _nearestText;
    [SerializeField] private TextMeshProUGUI _deliveryText;

    private Ingredient _currentHeld;
    private float _deliveryFadeTimer;

    private void OnEnable()
    {
        EventBus.Subscribe<PlayerCarryChangedEvent>(OnCarryChanged);
        EventBus.Subscribe<NearestInteractableChangedEvent>(OnNearestChanged);
        EventBus.Subscribe<IngredientStateChangedEvent>(OnStateChanged);
        EventBus.Subscribe<ItemDeliveredEvent>(OnDelivered);
        RefreshHeld();
        RefreshNearest(string.Empty);
        if (_deliveryText != null) _deliveryText.text = string.Empty;
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<PlayerCarryChangedEvent>(OnCarryChanged);
        EventBus.Unsubscribe<NearestInteractableChangedEvent>(OnNearestChanged);
        EventBus.Unsubscribe<IngredientStateChangedEvent>(OnStateChanged);
        EventBus.Unsubscribe<ItemDeliveredEvent>(OnDelivered);
    }

    private void Update()
    {
        if (_deliveryText == null) return;
        if (_deliveryFadeTimer > 0f)
        {
            _deliveryFadeTimer -= Time.deltaTime;
            if (_deliveryFadeTimer <= 0f) _deliveryText.text = string.Empty;
        }
    }

    private void OnCarryChanged(PlayerCarryChangedEvent e)
    {
        _currentHeld = e.Carried;
        RefreshHeld();
    }

    private void OnNearestChanged(NearestInteractableChangedEvent e)
    {
        RefreshNearest(e.Prompt);
    }

    private void OnStateChanged(IngredientStateChangedEvent e)
    {
        if (_currentHeld != null && e.Ingredient == _currentHeld) RefreshHeld();
    }

    private void OnDelivered(ItemDeliveredEvent e)
    {
        if (_deliveryText == null) return;
        _deliveryText.text = $"Delivered: {e.IngredientName} ({e.State})";
        _deliveryFadeTimer = 2.5f;
    }

    private void RefreshHeld()
    {
        if (_heldText == null) return;
        _heldText.text = _currentHeld != null
            ? $"Held: {_currentHeld.DisplayName} ({_currentHeld.State})"
            : "Held: —";
    }

    private void RefreshNearest(string prompt)
    {
        if (_nearestText == null) return;
        _nearestText.text = string.IsNullOrEmpty(prompt) ? "Near: —" : $"Near: {prompt}";
    }
}
