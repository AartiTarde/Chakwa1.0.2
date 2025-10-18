using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTile : MonoBehaviour
{

    /*public float rotatedirection;
    public static RotateTile  instance;
    void Start()
    {
        transform.Rotate(Vector3.up,-90);

    }

    public void degree90()
    {
        transform.Rotate(Vector3.up, 90);
    }
    public void degree30()
    {
        transform.Rotate(Vector3.up, 30);
    }
    public void degree60()
    {
        transform.Rotate(Vector3.up, 60);
    }
    public void degree120()
    {
        transform.Rotate(Vector3.up, 120);
    }
    public void degree10()
    {
        transform.Rotate(Vector3.up, 120);
    }*/
    public Transform target;

    void Start()
    {
        Vector3 relativePos = target.position + transform.position;
        
        
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.left);
        transform.rotation = rotation;
        print("rotation :"+transform.rotation);
    }
}
