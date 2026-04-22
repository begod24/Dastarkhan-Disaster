using UnityEngine;

public abstract class StationBase : MonoBehaviour, IInteractable
{
    [SerializeField] protected string _label;

    public string Label => string.IsNullOrEmpty(_label) ? name : _label;
    public virtual string InteractionPrompt => Label;
    public virtual bool CanInteract(PlayerController player) => true;
    public abstract void OnInteract(PlayerController player);
}
