using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed : MonoBehaviour
{
    public static Speed instance;

    void Awake()
    {
        instance = this;
    }
    public SpeedBoost speedBoostScript;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            speedBoostScript.ActivateSpeedBoost();
            Destroy(gameObject);
        }
    }
}
