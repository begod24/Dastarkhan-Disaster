using UnityEngine;

namespace DastarkhanDisaster.Gameplay.CameraRig
{
    /// <summary>
    /// Fixed top-down camera that smoothly follows a target.
    /// Pitch and offset are tweakable in the Inspector. No Cinemachine needed.
    /// </summary>
    public class TopDownCamera : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform _target;

        [Header("Framing")]
        [SerializeField] private Vector3 _offset = new Vector3(0f, 12f, -7f);
        [SerializeField] private float _pitch = 60f;
        [SerializeField] private float _smoothTime = 0.15f;

        private Vector3 _velocity;

        public void SetTarget(Transform target) => _target = target;

        private void LateUpdate()
        {
            if (_target == null) return;

            var desired = _target.position + _offset;
            transform.position = Vector3.SmoothDamp(
                transform.position, desired, ref _velocity, _smoothTime);
            transform.rotation = Quaternion.Euler(_pitch, 0f, 0f);
        }
    }
}
