using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public SoundDatabase soundDatabase;
    public Animator animator ;
  
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HapticManager.PlayHaptic(HapticManager.HapticType.Light);
            //CoinManager.instance.addCount();
            FindObjectOfType<GameManager>().AddCoin();
            if (animator!=null)
            {
                print("print  coin collection panel anim");
            }
           
            SoundManager.Instance.PlaySound(soundDatabase.coinSound);
            Destroy(gameObject);
        }
    }
}
