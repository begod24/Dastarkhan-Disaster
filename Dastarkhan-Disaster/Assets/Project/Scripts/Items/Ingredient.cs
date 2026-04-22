using UnityEngine;

public class Ingredient : MonoBehaviour, IInteractable
{
    [SerializeField] private IngredientSO _data;
    [SerializeField] private ProcessState _state;
    [SerializeField] private Renderer _renderer;

    public IngredientSO Data => _data;
    public ProcessState State => _state;
    public string DisplayName => _data != null ? _data.DisplayName : name;
    public string InteractionPrompt => $"Pick up {DisplayName}";

    private void Awake()
    {
        if (_renderer == null) _renderer = GetComponentInChildren<Renderer>();
    }

    public void Initialize(IngredientSO data)
    {
        _data = data;
        _state = data.InitialState;
        RefreshVisual();
    }

    public void SetState(ProcessState newState)
    {
        _state = newState;
        RefreshVisual();
        EventBus.Raise(new IngredientStateChangedEvent { Ingredient = this, NewState = newState });
    }

    public bool CanInteract(PlayerController player) => !player.Carry.IsCarrying;

    public void OnInteract(PlayerController player)
    {
        if (player.Carry.IsCarrying) return;
        player.Carry.Pickup(this);
    }

    private void RefreshVisual()
    {
        if (_renderer == null || _data == null || _data.StateVisuals == null) return;
        foreach (var sv in _data.StateVisuals)
        {
            if (sv.State == _state)
            {
                _renderer.material.color = sv.Tint;
                return;
            }
        }
    }
}
