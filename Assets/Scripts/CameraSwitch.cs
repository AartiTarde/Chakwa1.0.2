using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraSwitch : MonoBehaviour
{
    public CinemachineVirtualCamera introCam;
    public CinemachineVirtualCamera followCam;
    public float delay = 2.5f;

    void Start()
    {
        StartCoroutine(SwitchToFollowCam());
    }

    IEnumerator SwitchToFollowCam()
    {
       
        introCam.Priority = 20;
        followCam.Priority = 10;

        yield return new WaitForSeconds(delay);

        
        introCam.Priority = 10;
        followCam.Priority = 20;
    }
}  