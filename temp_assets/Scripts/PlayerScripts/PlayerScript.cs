/*using UnityEngine;
using System.Collections;

using Chakwa;
public class PlayerScript : MonoBehaviour
{
    private GameObject lastTrigger = null;
    public float forwardSpeed = 5f;
    public static PlayerScript Instance;
    void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        forwardSpeed = forwardSpeed + 0.2f;
        print("forward Speed increase :" + forwardSpeed);

        float speed = Mathf.Clamp(forwardSpeed, 42.84f, 48.72f);
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        transform.Translate(Vector3.right * speed * Time.deltaTime);


        //if (Input.GetKey(KeyCode.A))
        //{
        //    transform.Translate(Vector3.left * speed * Time.deltaTime);
        //    print("A key is pressed");
        //}
        //else if (Input.GetKey(KeyCode.D))
        //{
        //    transform.Translate(Vector3.forward * speed * Time.deltaTime);
        //    print("D key is pressed");
        //}
        //else if (Input.GetKey(KeyCode.W))
        //{
        //    transform.Translate(Vector3.back * speed * Time.deltaTime);
        //    print("W key is pressed");
        //}

    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TileTrigger"))
        {
            Path path = FindObjectOfType<Path>();
            if (path != null)
            {
                path.SpawnNextTile();
                print("trigger working");
            }

            Destroy(other.gameObject);

        }
    }*//*
    private void OnTriggerEnter(Collider other)
    {
        Path path = Path.Instance;

        if (path == null) return;

        if (other.CompareTag("TileTrigger"))
        {
            path.SpawnNextTile();
            Debug.Log("TileTrigger activated: Spawned next tile.");
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("turnleft"))
        {
            path.TriggerLeftTurn();
            Debug.Log("LeftTrigger activated: Spawned left turn tile.");
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("turnright"))
        {
            path.TriggerRightTurn();
            Debug.Log("RightTrigger activated: Spawned right turn tile.");
            Destroy(other.gameObject);
        }
        if (lastTrigger != null)
        {
            Destroy(lastTrigger);
        }

        // Update lastTrigger to current trigger for next deletion
        lastTrigger = other.gameObject;
    }

}*/


using UnityEngine;
using Chakwa;

public class PlayerScript : MonoBehaviour
{
   
    public static PlayerScript Instance;

    private GameObject lastTrigger = null;
    private float verticalVelocity;
    void Awake()
    {
        Instance = this;
    }
   
    //void Update()
    //{
    //    //forwardSpeed = forwardSpeed - 0.2f;


    //    //float speed = Mathf.Clamp(forwardSpeed, 42.84f, 48.72f);
    //    //transform.Translate(Vector3.right * speed * Time.deltaTime);
    //    //forwardSpeed = forwardSpeed +0.2f;
    //    //Vector3 move = Vector3.right * forwardSpeed + Vector3.up * verticalVelocity;

    //}

    private void OnTriggerEnter(Collider other)
    {
        Path path = Path.Instance;

        if (path == null) return;

        if (other.CompareTag("TileTrigger"))
        {
            path.SpawnNextTile();
            Debug.Log("TileTrigger activated: Spawned next tile.");
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("turnleft"))
        {
           path.TriggerLeftTurn();
            
            Debug.Log("LeftTrigger activated: Spawned left turn tile.");
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("turnright"))
        {
           path.TriggerRightTurn();
            Debug.Log("RightTrigger activated: Spawned right turn tile.");
            Destroy(other.gameObject);
        }
        
            if (lastTrigger != null)
        {
            Destroy(lastTrigger);
        }

        lastTrigger = other.gameObject;
    }
   

}

