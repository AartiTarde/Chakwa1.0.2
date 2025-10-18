using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
    private int coinCount = 0;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
       // PlayerPrefs.SetInt("Coins", 0);
        //PlayerPrefs.Save();
        LoadCoins();
        GameManager.Instance?.UpdateCoinUI(); 
    }
    
    public void addCount()
    {
        coinCount++;
        saveCoins(coinCount);


        Debug.Log("Add Coin Called, New Count: " + coinCount);

        GameManager.Instance?.UpdateCoinUI();
    }
    
    public void saveCoins(int coins)
    {
        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.Save();
        Debug.Log("save coins: " + coins);
        
    }
    public void LoadCoins()
    {
        coinCount = PlayerPrefs.GetInt("Coins",0);
        Debug.Log("Coins Loaded: " + coinCount); 
    }
   
    public int GetCoinCount()
    {
        return coinCount;
    }
   
    public void ResetCoins() { coinCount = 0; }

   
}
