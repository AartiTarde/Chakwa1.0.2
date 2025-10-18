using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScoringSystem : MonoBehaviour
{
   
    float distanceTravelled ;

    /*****
    Distance formula = speed*time = 30*10 ==300

    Time = distance/speed == 300*30 == 9000

    Speed = distance / time == 300*10 == 3000
    ******/


    void Start()
    {
        distanceTravelled = 1;
    }

    void Update()
    {
       
    }
    void score()
    {
        // float distancal= PlayerScript.Instance.forwardSpeed*Time.deltaTime;
        // distanceTravelled += PlayerScript.Instance.forwardSpeed * Time.deltaTime;
        CoinManager.instance.addCount();
        
        print("Distance Travlled : "+ distanceTravelled.ToString());
    }
}
