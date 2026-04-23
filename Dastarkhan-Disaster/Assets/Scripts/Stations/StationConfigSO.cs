using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Station_", menuName = "Dastarkhan/Station Config")]
public class StationConfigSO : ScriptableObject
{
    [SerializeField] private string _displayName;
    [SerializeField] private ProcessState _acceptsState;
    [SerializeField] private ProcessState _producesState;
    [SerializeField] private List<IngredientSO> _acceptedIngredients = new();
    [SerializeField] private float _processDuration;
    [SerializeField] private int _capacity = 1;
    [SerializeField] private bool _canBurn;
    [SerializeField] private float _burnDelay = 5f;

    public string DisplayName => _displayName;
    public ProcessState AcceptsState => _acceptsState;
    public ProcessState ProducesState => _producesState;
    public float ProcessDuration => _processDuration;
    public int Capacity => Mathf.Max(1, _capacity);
    public bool CanBurn => _canBurn;
    public float BurnDelay => _burnDelay;

    public bool AcceptsItem(Ingredient item)
    {
        if (item == null) return false;
        if (item.State != _acceptsState) return false;
        if (_acceptedIngredients.Count > 0 && !_acceptedIngredients.Contains(item.Data)) return false;
        return true;
    }
}
