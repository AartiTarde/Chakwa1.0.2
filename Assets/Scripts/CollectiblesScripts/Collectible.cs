using UnityEngine;

public class Collectible : MonoBehaviour
{
    public string collectibleTag; 
    public int value = 1;

    public SoundDatabase soundDatabase;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.Instance.PlaySound(soundDatabase.specialCoins);
            CollectibleInventory.Instance.AddCollectible(collectibleTag, value);
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            SoundManager.Instance.PlaySound(soundDatabase.specialCoins);
            CollectibleInventory.Instance.AddCollectible(collectibleTag, value);
            gameObject.SetActive(false);
        }
    }
}
