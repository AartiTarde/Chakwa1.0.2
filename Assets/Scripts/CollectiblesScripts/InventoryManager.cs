//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;
//public class InventoryManager : MonoBehaviour
//{
//    [System.Serializable]
//    public class CollectibleText
//    {
//        public string tag;                  // e.g. "trishul"
//        public TextMeshProUGUI textField;   // assign UI element in Inspector
//    }

//    [Header("Tag → Text Field Mapping")]
//    public List<CollectibleText> collectibleMappings = new List<CollectibleText>();

//    private Dictionary<string, TextMeshProUGUI> textDict;
//    public TextMeshProUGUI coinCounts;
//    void Awake()
//    {
//        // Build dictionary from list for easy lookup
//        textDict = new Dictionary<string, TextMeshProUGUI>();
//        foreach (var entry in collectibleMappings)
//        {
//            if (!string.IsNullOrEmpty(entry.tag) && entry.textField != null)
//            {
//                textDict[entry.tag] = entry.textField;
//            }
//        }
//    }

//    void Start()
//    {
//        UpdateUI();
//        int coinCount = PlayerPrefs.GetInt("Coins");
//        coinCounts.text = coinCount.ToString();
//    }

//    public void UpdateUI()
//    {
//        if (CollectibleInventory.Instance == null) return;

//        UpdateAsthiKalsh();
//        UpdateGangaJal();
//        UpdateKalsh();
//        UpdateRedNovel();
//        UpdateRudraksh();
//        UpdateTrishul();
//        UpdateTulsiMala();
//    }

//    void UpdateAsthiKalsh()
//    {
//        int count = CollectibleInventory.Instance.GetCount("asthikalsh");
//        if (textDict.ContainsKey("asthikalsh"))
//            textDict["asthikalsh"].text = count.ToString();
//    }

//    void UpdateGangaJal()
//    {
//        int count = CollectibleInventory.Instance.GetCount("gangajal");
//        if (textDict.ContainsKey("gangajal"))
//            textDict["gangajal"].text = count.ToString();
//    }

//    void UpdateKalsh()
//    {
//        int count = CollectibleInventory.Instance.GetCount("kalsh");
//        if (textDict.ContainsKey("kalsh"))
//            textDict["kalsh"].text =count.ToString();
//    }

//    void UpdateRedNovel()
//    {
//        int count = CollectibleInventory.Instance.GetCount("rednovel");
//        if (textDict.ContainsKey("rednovel"))
//            textDict["rednovel"].text = count.ToString();
//    }

//    void UpdateRudraksh()
//    {
//        int count = CollectibleInventory.Instance.GetCount("rudraksh");
//        if (textDict.ContainsKey("rudraksh"))
//            textDict["rudraksh"].text = count.ToString();
//    }

//    void UpdateTrishul()
//    {
//        int count = CollectibleInventory.Instance.GetCount("trishul");
//        if (textDict.ContainsKey("trishul"))
//            textDict["trishul"].text = count.ToString();
//    }

//    void UpdateTulsiMala()
//    {
//        int count = CollectibleInventory.Instance.GetCount("tulsimala");
//        if (textDict.ContainsKey("tulsimala"))
//            textDict["tulsimala"].text = count.ToString();
//    }

//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [System.Serializable]
    public class CollectibleText
    {
        public string tag;                  // e.g. "trishul"
        public TextMeshProUGUI textField;   // assign UI element in Inspector

        [Header("Redeem Settings")]
        public Button redeemButton;         // assign redeem button in Inspector
        public int coinCost = 10;           // cost in coins for redeem

        [Header("Image")]
        public Sprite inventoryItemImage;

        [Header("Rewarded Ads Settings")]
        public Button adsButton;            // assign ads button in Inspector
        public int adsRewardAmount = 1;     



    }

    
    [Header("Tag → Text Field Mapping")]
    public List<CollectibleText> collectibleMappings = new List<CollectibleText>();

    private Dictionary<string, TextMeshProUGUI> textDict;
    public TextMeshProUGUI coinCounts;

    void Awake()
    {
        // Build dictionary from list for easy lookup
        textDict = new Dictionary<string, TextMeshProUGUI>();
        foreach (var entry in collectibleMappings)
        {
            if (!string.IsNullOrEmpty(entry.tag) && entry.textField != null)
            {
                textDict[entry.tag] = entry.textField;

                // Add listener for redeem button if assigned
                if (entry.redeemButton != null)
                {
                    string currentTag = entry.tag; // capture tag for the lambda
                    int cost = entry.coinCost;
                    entry.redeemButton.onClick.AddListener(() => RedeemCollectible(currentTag, cost));
                }

                // Setup ads button
                if (entry.adsButton != null)
                {
                    string currentTag = entry.tag;
                    int rewardAmount = entry.adsRewardAmount;
                    entry.adsButton.onClick.AddListener(() => ShowRewardedCollectible(currentTag, rewardAmount));
                }
            }
        }
    }

    void Start()
    {
        UpdateUI();
        UpdateCoinUIAndButtons();
    }

    void Update()
    {
        // Keep coin count and button interactable status updated
        UpdateCoinUIAndButtons();
    }

    /// <summary>
    /// Update coin display and enable/disable redeem buttons based on player coins
    /// </summary>
    void UpdateCoinUIAndButtons()
    {
        int playerCoins = PlayerPrefs.GetInt("Coins");
        if (coinCounts != null)
            coinCounts.text = playerCoins.ToString();

        foreach (var entry in collectibleMappings)
        {
            if (entry.redeemButton != null)
            {
                entry.redeemButton.interactable = playerCoins >= entry.coinCost;
            }
        }
    }

    /// <summary>
    /// Redeem a collectible when button clicked
    /// </summary>
    /// <param name="tag">Collectible tag</param>
    /// <param name="coinCost">Cost in coins</param>
    public void RedeemCollectible(string tag, int coinCost)
    {
        int playerCoins = PlayerPrefs.GetInt("Coins", 0);
        if (playerCoins >= coinCost)
        {
            // Deduct coins
            playerCoins -= coinCost;
            PlayerPrefs.SetInt("Coins", playerCoins);
            PlayerPrefs.Save();

            // Add collectible
            if (CollectibleInventory.Instance != null)
            {
                CollectibleInventory.Instance.AddCollectible(tag, 1);
            }

            // Update UI
            UpdateUI();
            Debug.Log($"Redeemed 1 {tag}. Coins left: {playerCoins}");
        }
        else
        {
            Debug.Log("Not enough coins to redeem!");
        }
    }

    #region Existing Update Functions (Do Not Change)
    public void UpdateUI()
    {
        if (CollectibleInventory.Instance == null) return;

        UpdateAsthiKalsh();
        UpdateGangaJal();
        UpdateKalsh();
        UpdateRedNovel();
        UpdateRudraksh();
        UpdateTrishul();
        UpdateTulsiMala();
    }

    void UpdateAsthiKalsh()
    {
        int count = CollectibleInventory.Instance.GetCount("asthikalsh");
        if (textDict.ContainsKey("asthikalsh"))
            textDict["asthikalsh"].text = count.ToString();
    }

    void UpdateGangaJal()
    {
        int count = CollectibleInventory.Instance.GetCount("gangajal");
        if (textDict.ContainsKey("gangajal"))
            textDict["gangajal"].text = count.ToString();
    }

    void UpdateKalsh()
    {
        int count = CollectibleInventory.Instance.GetCount("kalsh");
        if (textDict.ContainsKey("kalsh"))
            textDict["kalsh"].text = count.ToString();
    }

    void UpdateRedNovel()
    {
        int count = CollectibleInventory.Instance.GetCount("rednovel");
        if (textDict.ContainsKey("rednovel"))
            textDict["rednovel"].text = count.ToString();
    }

    void UpdateRudraksh()
    {
        int count = CollectibleInventory.Instance.GetCount("rudraksh");
        if (textDict.ContainsKey("rudraksh"))
            textDict["rudraksh"].text = count.ToString();
    }

    void UpdateTrishul()
    {
        int count = CollectibleInventory.Instance.GetCount("trishul");
        if (textDict.ContainsKey("trishul"))
            textDict["trishul"].text = count.ToString();
    }

    void UpdateTulsiMala()
    {
        int count = CollectibleInventory.Instance.GetCount("tulsimala");
        if (textDict.ContainsKey("tulsimala"))
            textDict["tulsimala"].text = count.ToString();
    }

    /// <summary>
    /// Show rewarded ad → grant collectible if ad finished
    /// </summary>
    void ShowRewardedCollectible(string tag, int rewardAmount)
    {
        if (AdmobInitializer.instance == null)
        {
            Debug.LogError("AdmobInitializer not found in scene!");
            return;
        }

        AdmobInitializer.instance.ShowRewardedAd(() =>
        {
            if (CollectibleInventory.Instance != null)
            {
                CollectibleInventory.Instance.AddCollectible(tag, rewardAmount);
            }

            UpdateUI();
            Debug.Log($"✅ Player received {rewardAmount} {tag} from rewarded ad!");
        });
    }
    #endregion
}
