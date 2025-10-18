using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//using Chakwa.LevelPlayAds;

public class MainManager : MonoBehaviour, IPointerClickHandler
{
    [Header("UI Panels")]
    public GameObject mainPanel;
    public GameObject settingPanel;
    public GameObject mainMenuPanel;
    public GameObject quitPanel;
    public GameObject inventoryPanel;
    public GameObject mapPanel;
    //public Button myButton;
    public GameObject leaderBoardPanel;

    [Header("Sounds")]
    public SoundDatabase soundDatabase;

    public TMP_Text coinText;
    public TMP_Text privacypolicy;

    public ButtonAnimation playButtonAnim;
    void Start()
    {
        HideAllPanels();
        //myButton.onClick.AddListener(() => {
        //    Debug.Log("World space button clicked!");
        //});

        //LevelPlayAdManager.Instance.ShowBanner();
        AdmobInitializer.instance.ShowBanner();
        int coinCount = PlayerPrefs.GetInt("Coins", 0);
        print("MainMenuCoins" + coinCount);
        coinText.text = coinCount.ToString();
        
    }

    public void playGame()
    {
        //playButtonAnim.PlayClickAnimation();
        SceneManager.LoadScene("New Scene");
        SoundManager.Instance.PlaySound(soundDatabase.buttonSound);

    }
    public void setting()
    {
        settingPanel.SetActive(true);
        mainPanel.SetActive(false);
        SoundManager.Instance.PlaySound(soundDatabase.buttonSound);
    }

    public void backMainMenu()
    {
        settingPanel.SetActive(false);
        mainPanel.SetActive(true);
        SoundManager.Instance.PlaySound(soundDatabase.buttonSound);
    }
    public void QuitPanel()
    {
        quitPanel.SetActive(true);
    }

    public void quitYes()
    {
        #if UNITY_EDITOR
                            // Stop playing the scene in the editor
                            UnityEditor.EditorApplication.isPlaying = false;
        #else
                
                Application.Quit();
        #endif

    }
    public void quitNo()
    {
        if (quitPanel != null)
            quitPanel.SetActive(false);
            leaderBoardPanel.SetActive(false);
            mainPanel.SetActive(true);

    }

    public void showLeaderboard()
    {
        leaderBoardPanel.SetActive(true);
        mainPanel.SetActive(false);
    }

    public void inventory()
    {
        inventoryPanel.SetActive(true);
        mainPanel.SetActive(false);
    }
    private void HideAllPanels()
    {
        if (settingPanel != null)
            settingPanel.SetActive(false);
        if (mainPanel != null)
            mainPanel.SetActive(true);

        if (quitPanel != null)
            quitPanel.SetActive(false);

        if (leaderBoardPanel != null)
            leaderBoardPanel.SetActive(false);

        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);
        if (mapPanel != null)
            mapPanel.SetActive(false);
    }
    
    public void back()
    {
        mainPanel.SetActive(true);
        inventoryPanel.SetActive(false);
        settingPanel.SetActive(false);
        mapPanel.SetActive(false);
    }
    public void map()
    {

        mapPanel.SetActive(true);
        mainPanel.SetActive(false);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(privacypolicy, eventData.position, null);
        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = privacypolicy.textInfo.linkInfo[linkIndex];
            string url = "https://corelineitsolutions.github.io/privacypolicy/";
            Debug.Log("🌐 Opening privacy policy: " + url);
            Application.OpenURL(url);
        }
    }
}