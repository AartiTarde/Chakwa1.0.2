using System.Collections.Generic;
using UnityEngine;

public class CollectibleInventory : MonoBehaviour
{
    public static CollectibleInventory Instance;
    private Dictionary<string, int> collected = new Dictionary<string, int>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadCollectibles();

        int tulsiMala = CollectibleInventory.Instance.GetCount("trishul");
        print("You Have collect TulsiMala : "+tulsiMala);
    }
    void Start()
    {
    }

    public void AddCollectible(string tag, int value)
    {
        if (!collected.ContainsKey(tag))
            collected[tag] = 0;

        collected[tag] += value;

        PlayerPrefs.SetInt(tag, collected[tag]);  // save
        PlayerPrefs.Save();
        Debug.Log($"[CollectibleInventory] Collected {tag}, Total = {collected[tag]}");
    }

    public int GetCount(string tag)
    {
        return collected.ContainsKey(tag) ? collected[tag] : 0;
    }
    private void LoadCollectibles()
    {
        
        string[] tags = { "asthikalsh", "gangajal", "kalsh","rednovel","rudraksh","trishul","tulsimala"};
        foreach (string tag in tags)
        {
            int savedValue = PlayerPrefs.GetInt(tag, 0); 
            PlayerPrefs.Save();
            collected[tag] = savedValue;
            Debug.Log($"[CollectibleInventory] Loaded {tag} = {savedValue}");
        }
    }
}
