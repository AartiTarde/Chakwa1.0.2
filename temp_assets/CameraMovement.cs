using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraMovement : MonoBehaviour
{
    public float speed = 0.1f;
    public Vector3 vector;
    public GameObject[] straight;
    private void Start()
    {
        StartCoroutine(movement());
    }
    public IEnumerator movement() 
    {
        speed+=1.0f;
        transform.Translate(0,0, Time.deltaTime * speed);
        print("Translate : "+transform.position);    

        for (int i = 0; i <=3; )
        {
            Instantiate(straight[i],transform.right,Quaternion.identity);
        }
        yield return null;
    }
}
