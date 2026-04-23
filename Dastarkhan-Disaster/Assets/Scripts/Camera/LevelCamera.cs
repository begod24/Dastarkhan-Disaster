using UnityEngine;

namespace DastarkhanDisaster.Gameplay.CameraRig
{
    /// <summary>
    /// Static level camera. Position and rotation are set in the editor per level.
    /// No follow, no smoothing — just like Overcooked.
    /// </summary>
    public class LevelCamera : MonoBehaviour
    {
        [Header("Quick Setup (optional)")]
        [Tooltip("If set, camera will position itself relative to this point on Start.")]
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

        /// <summary>Call this to snap camera to a new level center (scene transitions).</summary>
        public void FrameLevel(Transform center, Vector3 offset, float pitch)
        {
            _levelCenter = center;
            _offset = offset;
            _pitch = pitch;
            transform.position = center.position + _offset;
            transform.rotation = Quaternion.Euler(_pitch, 0f, 0f);
        }
    }
}
