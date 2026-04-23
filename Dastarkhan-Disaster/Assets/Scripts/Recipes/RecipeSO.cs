using UnityEngine;

[CreateAssetMenu(fileName = "Recipe_", menuName = "Dastarkhan/Recipe")]
public class RecipeSO : ScriptableObject
{
    [SerializeField] private string _displayName;
    [SerializeField] private Sprite _icon;
    [SerializeField] private IngredientSO _ingredient;
    [SerializeField] private ProcessState _requiredState;
    [SerializeField] private float _timeLimit = 45f;
    [SerializeField] private int _baseScore = 100;

    public string DisplayName => _displayName;
    public Sprite Icon => _icon;
    public IngredientSO Ingredient => _ingredient;
    public ProcessState RequiredState => _requiredState;
    public float TimeLimit => _timeLimit;
    public int BaseScore => _baseScore;

    public bool Matches(Ingredient item)
    {
        if (item == null || item.Data == null) return false;
        return item.Data == _ingredient && item.State == _requiredState;
    }
}
