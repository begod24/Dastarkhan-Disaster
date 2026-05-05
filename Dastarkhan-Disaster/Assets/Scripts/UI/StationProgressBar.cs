using UnityEngine;
using UnityEngine.UI;

public class StationProgressBar : MonoBehaviour
{
    [SerializeField] private ProcessingStation _station;
    [SerializeField] private GameObject _root;
    [SerializeField] private Image _fill;
    [SerializeField] private Image _background;

    [Header("Colors")]
    [SerializeField] private Color _processingColor = new Color(0.95f, 0.75f, 0.3f);
    [SerializeField] private Color _readyColor = new Color(0.4f, 0.85f, 0.4f);
    [SerializeField] private Color _aboutToBurnColor = new Color(0.95f, 0.45f, 0.15f);
    [SerializeField] private Color _burnedColor = new Color(0.3f, 0.2f, 0.15f);
    [SerializeField] private Color _backgroundColor = new Color(0f, 0f, 0f, 0.55f);

    [Header("Behavior")]
    [SerializeField] private bool _faceCamera = true;
    [SerializeField] private bool _hideWhenEmpty = true;

    private Camera _camera;
    private Transform _cameraTransform;

    private void Awake()
    {
        if (_station == null) _station = GetComponentInParent<ProcessingStation>();
        if (_background != null) _background.color = _backgroundColor;
    }

    private void Start()
    {
        _camera = Camera.main;
        if (_camera != null) _cameraTransform = _camera.transform;
    }

    private void LateUpdate()
    {
        if (_station == null || _root == null || _fill == null) return;

        var visualState = _station.GetVisualState();

        if (_hideWhenEmpty && visualState == StationVisualState.Empty)
        {
            if (_root.activeSelf) _root.SetActive(false);
            return;
        }
        if (!_root.activeSelf) _root.SetActive(true);

        float fillAmount;
        Color color;

        switch (visualState)
        {
            case StationVisualState.Processing:
                fillAmount = _station.GetProgress();
                color = _processingColor;
                break;
            case StationVisualState.Ready:
                fillAmount = 1f;
                color = _readyColor;
                break;
            case StationVisualState.AboutToBurn:
                fillAmount = 1f;
                color = _aboutToBurnColor;
                break;
            case StationVisualState.Burned:
                fillAmount = 1f;
                color = _burnedColor;
                break;
            default:
                fillAmount = 0f;
                color = _processingColor;
                break;
        }

        _fill.fillAmount = fillAmount;
        _fill.color = color;

        if (_faceCamera && _cameraTransform != null)
        {
            transform.rotation = Quaternion.LookRotation(
                transform.position - _cameraTransform.position,
                Vector3.up);
        }
    }
}
