using UnityEngine;

namespace Chakwa
{
    /// <summary>
    /// Stores the revive (checkpoint) position for this tile.
    /// </summary>
    public class RevivePoint : MonoBehaviour
    {
        [Header("Revive Point Settings")]
        [Tooltip("Position where player will respawn when using this tile's checkpoint.")]
        public Transform reviveTransform;  

        private void Awake()
        {
            
            if (reviveTransform == null)
                reviveTransform = transform;
        }

    }
}
