using UnityEngine;
using DastarkhanDisaster.Core.Events;
using DastarkhanDisaster.Core.Interaction;

namespace DastarkhanDisaster.Gameplay.Player
{
    /// <summary>Raised whenever the nearest interactable changes (or its prompt changes).</summary>
    public struct NearestInteractableChangedEvent
    {
        public string TargetName;
        public string ActionPrompt;
    }

    /// <summary>Raised when the held object changes (picked up, dropped, transformed).</summary>
    public struct HeldItemChangedEvent
    {
        public string ItemName;
    }

    /// <summary>
    /// Detects the closest IInteractable in range and forwards Interact/Drop input to it.
    /// Depends only on the IInteractable interface (Core), not on station classes.
    /// SOLID: Open/Closed - new station types plug in without modifying this script.
    /// </summary>
    [RequireComponent(typeof(PlayerInputHandler))]
    [RequireComponent(typeof(PlayerCarry))]
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("Detection")]
        [SerializeField] private float _detectRadius = 1.5f;
        [SerializeField] private LayerMask _interactableLayer = ~0;

        private PlayerInputHandler _input;
        private PlayerCarry _carry;
        private IInteractable _nearest;
        private string _lastPrompt;

        private static readonly Collider[] _hitBuffer = new Collider[16];

        private void Awake()
        {
            _input = GetComponent<PlayerInputHandler>();
            _carry = GetComponent<PlayerCarry>();
        }

        private void OnEnable()
        {
            _input.OnInteractPressed += HandleInteract;
            _input.OnDropPressed += HandleDrop;
        }

        private void OnDisable()
        {
            _input.OnInteractPressed -= HandleInteract;
            _input.OnDropPressed -= HandleDrop;
        }

        private void Update()
        {
            DetectNearest();
            BroadcastPromptIfChanged();
        }

        private void DetectNearest()
        {
            int count = Physics.OverlapSphereNonAlloc(
                transform.position, _detectRadius, _hitBuffer, _interactableLayer);

            IInteractable best = null;
            float bestDist = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                var col = _hitBuffer[i];
                var interactable = col.GetComponentInParent<IInteractable>();
                if (interactable == null) continue;

                var mb = col.GetComponentInParent<MonoBehaviour>();
                if (mb == null) continue;

                float d = (mb.transform.position - transform.position).sqrMagnitude;
                if (d < bestDist) { bestDist = d; best = interactable; }
            }

            _nearest = best;
        }

        private void BroadcastPromptIfChanged()
        {
            string prompt = _nearest?.GetActionPrompt(_carry.HeldObject) ?? "—";
            string targetName = _nearest is MonoBehaviour mb ? mb.name : "None";

            if (prompt == _lastPrompt) return;
            _lastPrompt = prompt;

            EventBus.Raise(new NearestInteractableChangedEvent
            {
                TargetName = targetName,
                ActionPrompt = prompt
            });
        }

        private void HandleInteract()
        {
            if (_nearest == null) return;
            var result = _nearest.Interact(_carry.HeldObject, _carry.CarryAnchor);

            if (result != _carry.HeldObject)
            {
                _carry.Hold(result);
                EventBus.Raise(new HeldItemChangedEvent
                {
                    ItemName = result != null ? result.name : "—"
                });
            }
            // Force prompt refresh (state may have changed)
            _lastPrompt = null;
        }

        private void HandleDrop()
        {
            if (!_carry.HasItem) return;
            var dropped = _carry.ReleaseHeld();
            Destroy(dropped); // prototype: drop = vaporize. Will route to ground/counters in Stage 6.
            EventBus.Raise(new HeldItemChangedEvent { ItemName = "—" });
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _detectRadius);
        }
    }
}
