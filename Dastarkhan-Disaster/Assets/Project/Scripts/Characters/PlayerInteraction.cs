using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float _radius = 1.5f;
    [SerializeField] private LayerMask _interactableLayer = ~0;
    [SerializeField] private Transform _probeOrigin;
    [SerializeField] private int _playerId;

    private PlayerController _player;
    private PlayerInputHandler _input;
    private IInteractable _nearest;

    private readonly Collider[] _buffer = new Collider[16];

    private void Awake()
    {
        _player = GetComponent<PlayerController>();
        _input = GetComponent<PlayerInputHandler>();
        if (_probeOrigin == null) _probeOrigin = transform;
    }

    private void Update()
    {
        UpdateNearest();

        if (_input.InteractPressed && _nearest != null && _nearest.CanInteract(_player))
            _nearest.OnInteract(_player);
    }

    private void UpdateNearest()
    {
        int count = Physics.OverlapSphereNonAlloc(
            _probeOrigin.position, _radius, _buffer, _interactableLayer, QueryTriggerInteraction.Collide);

        IInteractable best = null;
        float bestDist = float.MaxValue;

        for (int i = 0; i < count; i++)
        {
            var col = _buffer[i];
            if (col == null) continue;
            if (col.transform == transform || col.transform.IsChildOf(transform)) continue;

            var interactable = col.GetComponentInParent<IInteractable>();
            if (interactable == null) continue;

            float d = (col.transform.position - _probeOrigin.position).sqrMagnitude;
            if (d < bestDist)
            {
                bestDist = d;
                best = interactable;
            }
        }

        if (!ReferenceEquals(best, _nearest))
        {
            _nearest = best;
            EventBus.Raise(new NearestInteractableChangedEvent
            {
                PlayerId = _playerId,
                Interactable = _nearest,
                Prompt = _nearest != null ? _nearest.InteractionPrompt : string.Empty
            });
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        var origin = _probeOrigin != null ? _probeOrigin.position : transform.position;
        Gizmos.DrawWireSphere(origin, _radius);
    }
}
