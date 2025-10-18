using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class MainManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainPanel;
    public GameObject settingPanel;

    public Button myButton;

 

    void Start()
    {
        HideAllPanels();
        myButton.onClick.AddListener(() => {
            Debug.Log("World space button clicked!");
        });
    }

    public void playGame()
    {
        SceneManager.LoadScene("New Scene");
    }
    public void setting()
    {
        settingPanel.SetActive(true);   
    }
    private void HideAllPanels()
    {
        if (settingPanel != null)
            settingPanel.SetActive(false);
        if (mainPanel != null)
            mainPanel.SetActive(true);
    }
}