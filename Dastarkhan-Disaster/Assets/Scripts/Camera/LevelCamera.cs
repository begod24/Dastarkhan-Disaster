using UnityEngine;

public class LevelCamera : MonoBehaviour
{
    [Header("Quick Setup (optional)")]
    [SerializeField] private Transform _levelCenter;
    [SerializeField] private Vector3 _offset = new Vector3(0f, 8f, -5f);
    [SerializeField] private float _pitch = 50f;

    private void Start()
    {
        if (_levelCenter != null)
        {
            transform.position = _levelCenter.position + _offset;
            transform.rotation = Quaternion.Euler(_pitch, 0f, 0f);
        }
    }

    public void FrameLevel(Transform center, Vector3 offset, float pitch)
    {
        _levelCenter = center;
        _offset = offset;
        _pitch = pitch;
        transform.position = center.position + _offset;
        transform.rotation = Quaternion.Euler(_pitch, 0f, 0f);
    }
}
