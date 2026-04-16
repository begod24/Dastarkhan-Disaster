using UnityEngine;

namespace DastarkhanDisaster.Gameplay.Player
{
    /// <summary>
    /// Stores and parents a single carried GameObject to a hand/anchor transform.
    /// Knows nothing about food, recipes, or stations.
    /// SOLID: Single Responsibility - only manages the held reference + parenting.
    /// </summary>
    public class PlayerCarry : MonoBehaviour
    {
        [SerializeField] private Transform _carryAnchor;
        public Transform CarryAnchor => _carryAnchor;

        public GameObject HeldObject { get; private set; }
        public bool HasItem => HeldObject != null;

        public void Hold(GameObject obj)
        {
            HeldObject = obj;
            if (obj == null) return;
            obj.transform.SetParent(_carryAnchor);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
        }

        public GameObject ReleaseHeld()
        {
            var obj = HeldObject;
            HeldObject = null;
            if (obj != null) obj.transform.SetParent(null);
            return obj;
        }
    }
}
