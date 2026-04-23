using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderCardUI : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Image _timeFill;
    [SerializeField] private Image _background;
    [SerializeField] private Color _normalColor = new Color(0.15f, 0.12f, 0.08f, 0.95f);
    [SerializeField] private Color _urgentColor = new Color(0.5f, 0.1f, 0.05f, 0.95f);
    [SerializeField] private Color _fillNormal = new Color(0.85f, 0.7f, 0.35f);
    [SerializeField] private Color _fillUrgent = new Color(0.9f, 0.2f, 0.15f);
    [SerializeField] private float _urgentThreshold = 0.3f;

    public int OrderId { get; private set; }

    public void Bind(ActiveOrder order)
    {
        OrderId = order.Id;
        if (_nameText != null) _nameText.text = order.Recipe.DisplayName;
        if (_iconImage != null)
        {
            if (order.Recipe.Icon != null)
            {
                _iconImage.sprite = order.Recipe.Icon;
                _iconImage.enabled = true;
            }
            else
            {
                _iconImage.enabled = false;
            }
        }
        UpdateTime(order);
    }

    public void UpdateTime(ActiveOrder order)
    {
        float t = Mathf.Clamp01(order.NormalizedTime);
        if (_timeFill != null)
        {
            _timeFill.fillAmount = t;
            _timeFill.color = t < _urgentThreshold ? _fillUrgent : _fillNormal;
        }
        if (_background != null)
        {
            _background.color = t < _urgentThreshold ? _urgentColor : _normalColor;
        }
    }
}
