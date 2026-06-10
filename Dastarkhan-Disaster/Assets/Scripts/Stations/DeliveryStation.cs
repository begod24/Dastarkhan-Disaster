using UnityEngine;

public class DeliveryStation : StationBase
{
    public override string InteractionPrompt => $"{Label}: Deliver";

    public override bool CanInteract(PlayerController player) =>
        player.Carry.CarriedIngredient != null || player.Carry.CarriedPlate != null;

    public override void OnInteract(PlayerController player)
    {
        var plate = player.Carry.CarriedPlate;
        if (plate != null)
        {
            DeliverPlate(plate);
            return;
        }

        var item = player.Carry.CarriedIngredient;
        if (item == null) return;
        DeliverIngredient(player, item);
    }

    private void DeliverIngredient(PlayerController player, Ingredient item)
    {
        player.Carry.Drop();

        bool delivered = false;
        int awarded = 0;
        ActiveOrder matched = null;

        if (OrderManager.Instance != null)
            delivered = OrderManager.Instance.TryDeliver(item, out matched, out awarded);

        EventBus.Raise(new ItemDeliveredEvent
        {
            IngredientName = item.Data != null ? item.Data.DisplayName : item.name,
            State = item.State
        });

        if (!delivered) Debug.Log($"[Delivery] No matching order for {item.Data?.DisplayName} ({item.State})");
        else Debug.Log($"[Delivery] Completed order #{matched.Id} (+{awarded})");

        Destroy(item.gameObject);
    }

    private void DeliverPlate(Plate plate)
    {
        bool delivered = false;
        int awarded = 0;
        ActiveOrder matched = null;

        if (OrderManager.Instance != null)
            delivered = OrderManager.Instance.TryDeliver(plate, out matched, out awarded);

        if (!delivered)
        {
            Debug.Log($"[Delivery] No matching order for {plate.DisplayName} ({plate.Contents.Count} items)");
            return;
        }

        EventBus.Raise(new ItemDeliveredEvent
        {
            IngredientName = matched.DisplayName,
            State = ProcessState.Raw
        });

        Debug.Log($"[Delivery] Completed plated order #{matched.Id} (+{awarded})");
        plate.Clear();
    }
}
