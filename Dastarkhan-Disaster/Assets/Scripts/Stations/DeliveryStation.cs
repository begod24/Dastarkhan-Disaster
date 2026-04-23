using UnityEngine;

public class DeliveryStation : StationBase
{
    public override string InteractionPrompt => $"{Label}: Deliver";
    public override bool CanInteract(PlayerController player) => player.Carry.IsCarrying;

    public override void OnInteract(PlayerController player)
    {
        if (!player.Carry.IsCarrying) return;

        var item = player.Carry.Drop();

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
}
