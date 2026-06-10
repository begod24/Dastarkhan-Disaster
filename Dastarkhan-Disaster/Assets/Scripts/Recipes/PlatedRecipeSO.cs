using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe_", menuName = "Dastarkhan/Plated Recipe")]
public class PlatedRecipeSO : ScriptableObject
{
    [System.Serializable]
    public struct Requirement
    {
        public IngredientSO Ingredient;
        public ProcessState State;
    }

    [SerializeField] private string _displayName;
    [SerializeField] private Sprite _icon;
    [SerializeField] private List<Requirement> _requirements = new();
    [SerializeField] private float _timeLimit = 60f;
    [SerializeField] private int _baseScore = 200;

    public string DisplayName => _displayName;
    public Sprite Icon => _icon;
    public IReadOnlyList<Requirement> Requirements => _requirements;
    public float TimeLimit => _timeLimit;
    public int BaseScore => _baseScore;

    public bool Matches(IReadOnlyList<Ingredient> contents)
    {
        if (contents == null || _requirements.Count == 0) return false;
        if (contents.Count != _requirements.Count) return false;

        var remaining = new List<Requirement>(_requirements);
        for (int i = 0; i < contents.Count; i++)
        {
            var item = contents[i];
            if (item == null || item.Data == null) return false;

            int found = -1;
            for (int j = 0; j < remaining.Count; j++)
            {
                if (remaining[j].Ingredient == item.Data && remaining[j].State == item.State)
                {
                    found = j;
                    break;
                }
            }
            if (found < 0) return false;
            remaining.RemoveAt(found);
        }
        return remaining.Count == 0;
    }
}
