using UnityEngine;

public class PlateDispenser : StationBase
{
    [SerializeField] private GameObject _platePrefab;
    [SerializeField] private Transform _spawnAnchor;

    public override string InteractionPrompt => $"{Label}: Take plate";

    public override bool CanInteract(PlayerController player) =>
        _platePrefab != null && !player.Carry.IsCarrying;

    public override void OnInteract(PlayerController player)
    {
        if (_platePrefab == null)
        {
            Debug.LogWarning($"[{name}] PlateDispenser has no plate prefab assigned.");
            return;
        }
        if (player.Carry.IsCarrying) return;

        Vector3 pos = _spawnAnchor != null ? _spawnAnchor.position : transform.position + Vector3.up * 0.5f;
        GameObject go = Instantiate(_platePrefab, pos, Quaternion.identity);

        if (!go.TryGetComponent<Plate>(out var plate))
            plate = go.AddComponent<Plate>();

        player.Carry.Pickup(plate);
    }

    private void OnDrawGizmos()
    {
        if (_spawnAnchor == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_spawnAnchor.position, 0.15f);
    }
}
