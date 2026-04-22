public struct IngredientStateChangedEvent
{
    public Ingredient Ingredient;
    public ProcessState NewState;
}

public struct PlayerCarryChangedEvent
{
    public int PlayerId;
    public Ingredient Carried;
}

public struct NearestInteractableChangedEvent
{
    public int PlayerId;
    public IInteractable Interactable;
    public string Prompt;
}

public struct ItemDeliveredEvent
{
    public string IngredientName;
    public ProcessState State;
}
