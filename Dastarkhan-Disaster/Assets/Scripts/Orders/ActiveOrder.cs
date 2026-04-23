public class ActiveOrder
{
    public int Id;
    public RecipeSO Recipe;
    public float TimeRemaining;
    public float TimeLimit;
    public OrderState State;

    public float NormalizedTime => TimeLimit > 0f ? TimeRemaining / TimeLimit : 0f;
}

public enum OrderState
{
    Pending,
    Delivered,
    Expired
}
