using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraLockY : MonoBehaviour
{
    public float fixedHeight = 50f; // Your desired Y height
    private CinemachineVirtualCamera vcam;

    void Awake()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
    }

    void LateUpdate()
    {
        if (vcam == null || vcam.Follow == null) return;

        // Get the camera position after Cinemachine moves it
        Transform camTransform = vcam.VirtualCameraGameObject.transform;

        Vector3 pos = camTransform.position;
        pos.y = fixedHeight; // Force Y to be constant
        camTransform.position = pos;
    }
}
