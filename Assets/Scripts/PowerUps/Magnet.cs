using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public PowerUpsData powerUpsData;
    //public float duration = 5f;  // How long the magnet effect lasts
    //private bool isMagnetActive = false;
    public SoundDatabase soundDatabase;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Activate the magnet power-up effect on the player
            //PlayerMovement.Instance.ActivateMagnet(powerUpsData.runpowerupTime);
            //SoundManager.Instance.PlaySound(soundDatabase.magnetSound);
            CoinManager.instance.addCount();
            print("Magnet Working Magnet Script");
            // Destroy the power-up after it’s collected
            Destroy(gameObject);
        }
    }
}
