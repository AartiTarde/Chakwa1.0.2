using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadingScript : MonoBehaviour
{
    public float loadingSpeed = 0.5f;
    private bool isConnected = false;
    public Slider loadingSlider;
    public TMP_Text loadingText;

    [Header("SplashScreen")]
    public GameObject loadingPanel;
    public GameObject splashPanel;

    public string sceneName = "";
    void Start()
    {
        StartCoroutine(CheckInternetAndLoad());
        loadingPanel.SetActive(true);
    }
    IEnumerator CheckInternetAndLoad()
    {
        float progress = 0f;

        while (progress < 1f)
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                isConnected = true;
                progress += loadingSpeed * Time.deltaTime; // Increment progress
            }
            else
            {
                isConnected = false;
                //isConnected = true;
                //progress += loadingSpeed * Time.deltaTime;
            }

            // Update UI
            loadingSlider.value = progress;
            loadingText.text = Mathf.RoundToInt(progress * 100) + "%";

            yield return null;
        }

        // Load the next scene when the loading bar is full
        if (progress >= 1f)
        {
            loadingPanel.SetActive(false);
            splashPanel.SetActive(true);

            // Wait 3 sec then go to next scene
            yield return new WaitForSeconds(3f);

            SceneManager.LoadScene(sceneName);
        }
    }


   
}
