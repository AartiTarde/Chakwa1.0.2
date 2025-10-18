using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public float normalSpeed = 5f;
    public float boostSpeed = 10f;
    private float currentSpeed;

    private bool isBoosted = false;
    private float boostDuration = 5f;
    private float boostTimer = 0f;
    public static SpeedBoost instance;
    private void Start()
    {
        currentSpeed = normalSpeed;
    }
    private void Update()
    {
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
        if (isBoosted)
        {
            boostTimer -= Time.deltaTime;
            if (boostTimer <= 0f)
            {
                DeactivateSpeedBoost();
            }
        }
    }
    public void ActivateSpeedBoost()
    {
        if (!isBoosted)
        {
            print("Activated!!");
            isBoosted = true;
            currentSpeed = boostSpeed;
            boostTimer = boostDuration;
        }
    }
    private void DeactivateSpeedBoost()
    {
        isBoosted = false;
        currentSpeed = normalSpeed;
    }
}
