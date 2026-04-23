public interface IInteractable
{
    string InteractionPrompt { get; }
    bool CanInteract(PlayerController player);
    void OnInteract(PlayerController player);
}
