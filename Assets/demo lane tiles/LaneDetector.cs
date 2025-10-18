using UnityEngine;

public class LaneDetector : MonoBehaviour
{
    [Header("Lane LayerMasks")]
    public LayerMask leftlane;
    public LayerMask centerlane;
    public LayerMask rightlane;

    [Header("Ray Settings")]
    public float rayLength = 2f;  // adjust based on your scene

    void Update()
    {
        DetectLane();
    }

    void DetectLane()
    {
        Ray ray = new Ray(transform.position, Vector3.down); // cast downward
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayLength))
        {
            int hitLayer = hit.collider.gameObject.layer;

            if (IsInLayerMask(hitLayer, leftlane))
            {
                Debug.Log("Player is on LEFT lane");
            }
            else if (IsInLayerMask(hitLayer, centerlane))
            {
                Debug.Log("Player is on CENTER lane");
            }
            else if (IsInLayerMask(hitLayer, rightlane))
            {
                Debug.Log("Player is on RIGHT lane");
            }
        }
    }

    bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        return ((layerMask.value & (1 << layer)) != 0);
    }
}
