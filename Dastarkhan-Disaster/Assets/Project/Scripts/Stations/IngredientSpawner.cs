using UnityEngine;

public class IngredientSpawner : StationBase
{
    [SerializeField] private IngredientSO _ingredient;
    [SerializeField] private Transform _spawnAnchor;

    public override string InteractionPrompt =>
        _ingredient != null ? $"Take {_ingredient.DisplayName}" : Label;

    public override bool CanInteract(PlayerController player) =>
        _ingredient != null && !player.Carry.IsCarrying;

    public override void OnInteract(PlayerController player)
    {
        if (_ingredient == null || _ingredient.Prefab == null)
        {
            Debug.LogWarning($"[{name}] IngredientSpawner missing ingredient or prefab.");
            return;
        }
        if (player.Carry.IsCarrying) return;

        Vector3 pos = _spawnAnchor != null ? _spawnAnchor.position : transform.position + Vector3.up * 0.5f;
        GameObject go = Instantiate(_ingredient.Prefab, pos, Quaternion.identity);

        if (!go.TryGetComponent<Ingredient>(out var ing))
            ing = go.AddComponent<Ingredient>();

        ing.Initialize(_ingredient);
        player.Carry.Pickup(ing);
    }

    private void OnDrawGizmos()
    {
        if (_spawnAnchor == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_spawnAnchor.position, 0.15f);
    }
}
