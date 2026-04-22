using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient_", menuName = "Dastarkhan/Ingredient")]
public class IngredientSO : ScriptableObject
{
    [SerializeField] private string _displayName;
    [SerializeField] private ProcessState _initialState = ProcessState.Raw;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private StateVisual[] _stateVisuals;

    public string DisplayName => _displayName;
    public ProcessState InitialState => _initialState;
    public GameObject Prefab => _prefab;
    public StateVisual[] StateVisuals => _stateVisuals;

    [System.Serializable]
    public struct StateVisual
    {
        public ProcessState State;
        public Color Tint;
    }
}
