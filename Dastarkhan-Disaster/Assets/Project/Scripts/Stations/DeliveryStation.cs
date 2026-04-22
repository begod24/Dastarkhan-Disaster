using UnityEngine;

public class DeliveryStation : StationBase
{
    public override string InteractionPrompt => $"{Label}: Deliver";
    public override bool CanInteract(PlayerController player) => player.Carry.IsCarrying;

    public override void OnInteract(PlayerController player)
    {
        if (!player.Carry.IsCarrying) return;

        var item = player.Carry.Drop();
        EventBus.Raise(new ItemDeliveredEvent
        {
            IngredientName = item.Data != null ? item.Data.DisplayName : item.name,
            State = item.State
        });
        Destroy(item.gameObject);
    }
}
