using UnityEngine;

namespace DastarkhanDisaster.Core.Interaction
{
    /// <summary>
    /// Contract for anything the player can press E on.
    /// Lives in Core so the Player module never needs to know about Stations.
    /// Pattern: Polymorphism via interface.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>Human-readable hint shown in debug HUD when player is in range.</summary>
        string GetActionPrompt(GameObject heldObject);

        /// <summary>
        /// Performs the interaction. Returns the GameObject the player should now be holding
        /// (the same one, a new one, or null if hands should be empty).
        /// </summary>
        GameObject Interact(GameObject heldObject, Transform carryAnchor);
    }
}
