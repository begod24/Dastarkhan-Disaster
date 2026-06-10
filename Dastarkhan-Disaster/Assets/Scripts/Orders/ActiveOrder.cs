using UnityEngine;

public class ActiveOrder
{
    public int Id;
    public RecipeSO Recipe;
    public PlatedRecipeSO PlatedRecipe;
    public float TimeRemaining;
    public float TimeLimit;
    public OrderState State;

    public bool IsPlated => PlatedRecipe != null;
    public string DisplayName => IsPlated ? PlatedRecipe.DisplayName : (Recipe != null ? Recipe.DisplayName : "");
    public Sprite Icon => IsPlated ? PlatedRecipe.Icon : (Recipe != null ? Recipe.Icon : null);
    public int BaseScore => IsPlated ? PlatedRecipe.BaseScore : (Recipe != null ? Recipe.BaseScore : 0);

    public float NormalizedTime => TimeLimit > 0f ? TimeRemaining / TimeLimit : 0f;
}

public enum OrderState
{
    Pending,
    Delivered,
    Expired
}
