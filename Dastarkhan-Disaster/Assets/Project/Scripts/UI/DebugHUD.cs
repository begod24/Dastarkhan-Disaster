using UnityEngine;
using TMPro;
using DastarkhanDisaster.Core.Events;
using DastarkhanDisaster.Gameplay.Player;

namespace DastarkhanDisaster.UI
{
    /// <summary>
    /// Minimal on-screen debug overlay.
    /// Pure Observer: subscribes to EventBus, never polls or holds Player references.
    /// In Stage 6 we'll add Recipe + Process state lines.
    /// </summary>
    public class DebugHUD : MonoBehaviour
    {
        [Header("TMP Targets")]
        [SerializeField] private TMP_Text _heldText;
        [SerializeField] private TMP_Text _nearestText;

        private void OnEnable()
        {
            EventBus.Subscribe<HeldItemChangedEvent>(OnHeldChanged);
            EventBus.Subscribe<NearestInteractableChangedEvent>(OnNearestChanged);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<HeldItemChangedEvent>(OnHeldChanged);
            EventBus.Unsubscribe<NearestInteractableChangedEvent>(OnNearestChanged);
        }

        private void Start()
        {
            if (_heldText != null)    _heldText.text = "Held: —";
            if (_nearestText != null) _nearestText.text = "Near: —";
        }

        private void OnHeldChanged(HeldItemChangedEvent e)
        {
            if (_heldText != null) _heldText.text = $"Held: {e.ItemName}";
        }

        private void OnNearestChanged(NearestInteractableChangedEvent e)
        {
            if (_nearestText != null) _nearestText.text = $"Near: {e.TargetName}\n{e.ActionPrompt}";
        }
    }
}
