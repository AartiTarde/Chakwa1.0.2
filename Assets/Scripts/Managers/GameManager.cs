using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;
//using Chakwa.LevelPlayAds;
using Chakwa.Player;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerMovement players;
    [Header("UI Panels")]
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    //public GameObject transitionScreen;
    public GameObject scoringPanel;
    public GameObject adsPanel;
    public GameObject inventoryPanel;

    [Header("Score UI")]
    public TMP_Text tmpScoreText;
    public TMP_Text coinTextDisplay;
    public TMP_Text tmpDistanceText;

    [Header("Game State")]
    private float score = 0f;
    private bool isPaused = false;
    private bool isGameOver = false;

    [Header("Player Tracking")]
    public Transform player;
    private float startX;

    [Header("GameOverPanel")]
    public TMP_Text survivaltextScore;
    public TMP_Text distanceText;
    public TMP_Text coinText;
    public TMP_Text finalTimeText;

    [Header("Paused Panel")]
    public TMP_Text pSurvivalScore;
    public TMP_Text pDistanceText;
    //public TMP_Text pCoin;
    public TMP_Text pTimeText;

    [Header("Sounds")]
    public SoundDatabase soundDatabase;

    [Header("Countdown UI")]
    public TMP_Text countdownText;


    [Header("Gameplay Timer")]
    public TMP_Text playTimeText;   
    private float playTime = 0f;

    private Coroutine countdownRoutine;
    private Coroutine submitRoutine;

    public CameraFollow cameraFollow;
    private int sessionCoins = 0;

    public int scoring;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        //else
        //{
        //    Destroy(gameObject);
        //}
    }

    void Start()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player")?.transform;

        if (player != null)
            startX = player.position.x;

        UpdateCoinUI();
        ResumeGameInstant(); // start in resumed state
        HideAllPanels();
        UpdateDistanceUI();

        submitRoutine = StartCoroutine(SubmitScorePeriodically());

        ResetScoreAndCoins();
    }
    void Update()
    {
        if (!isPaused && !isGameOver)
        {
            score += Time.deltaTime * 10f;
            if (ScoreManager.instance != null)
                ScoreManager.instance.SetScore(Mathf.FloorToInt(score));
            //UpdateScoreUI();
            UpdateDistanceUI();
            playTime += Time.deltaTime; 
            print("Time Function :  " + playTime);
            UpdatePlayTimeUI();
            UpdateCoinUI();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }
    // ------------------ UI Management ------------------
    private void HideAllPanels()
    {
        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (pausePanel) pausePanel.SetActive(false);
        if (scoringPanel) scoringPanel.SetActive(true);
        if (adsPanel) adsPanel.SetActive(false);

        if (inventoryPanel) inventoryPanel.SetActive(false);
    }
    private void UpdateScoreUI()
    {
        if (tmpScoreText)
            scoring = Mathf.FloorToInt(score);
            print("Final Scroing :" + scoring);
            tmpScoreText.text = "Score: " + scoring;
    }

    private void UpdateDistanceUI()
    {

        if (DistanceManager.instance != null)
        {
            int distance = ScoreManager.instance.GetScore();
            tmpDistanceText.text = "Score: " + distance.ToString();
            print("Distancce covered : " + distance.ToString());
        }
    }

    public void UpdateCoinUI()
    {
        if (coinTextDisplay)
        {
            // Show session coins only
            coinTextDisplay.text = sessionCoins.ToString();
            print("Session Coins (UI): " + sessionCoins);
        }

        if (tmpDistanceText && DistanceManager.instance)
        {
            int distance = ScoreManager.instance.GetScore();
            tmpDistanceText.text = "" + distance.ToString();
            PlayerPrefs.SetInt("Scoring", distance);
            print("Score manager is work " + distance);
        }
    }
    // Call this when player collects a coin
    public void AddCoin()
    {
        sessionCoins++;  // increase only session coins
        CoinManager.instance.addCount(); // also increase persistent coins
        UpdateCoinUI();
    }
    // ------------------ Game Flow ------------------
    public void GameOver()
    {
        isGameOver = true;
        isPaused = true;
        //PlayerMovement.Instance.enabled=false;
        //  Time.timeScale = 0f;

        // ShowInterstitialAd();

        if (gameOverPanel) gameOverPanel.SetActive(true);
        if (scoringPanel) scoringPanel.SetActive(false);
        if (adsPanel) adsPanel.SetActive(false);

        if (coinText && CoinManager.instance)
        {
           
            distanceText.text = DistanceManager.instance.GetDistance().ToString(); 


            if (finalTimeText != null || survivaltextScore != null || distanceText != null)
            {
                finalTimeText.text = FormatTime(playTime);

                if (coinText!=null && CoinManager.instance)
                {
                    int coin = CoinManager.instance.GetCoinCount();
                    coinText.text = ""+ coin.ToString();
                }

                if (distanceText != null && DistanceManager.instance)
                {
                    int distance = DistanceManager.instance.GetDistance();
                    distanceText.text = ""+ distance.ToString();
                }


                //if (survivaltextScore!=null && SurvivalManager.instance)
                //{
                //    survivaltextScore.text = SurvivalManager.instance.GetTimeSurvived().ToString();
                //    print("Survival Score :" + SurvivalManager.instance.GetTimeSurvived().ToString());
                //}
                if (survivaltextScore != null && SurvivalManager.instance)
                {
                    float survivalTimeFloat = SurvivalManager.instance.GetTimeSurvived();
                    int survivalTime = (int)survivalTimeFloat;
                    survivaltextScore.text = survivalTime.ToString();
                    print("Survival Score : " + survivalTime);
                }
                finalScoreLeaderboard();
            }
        }
    }   
    public void PauseGame()
    {
        Time.timeScale = 0f;
        if (pausePanel) pausePanel.SetActive(true);
        if (scoringPanel) scoringPanel.SetActive(false);
    }
    public void ResumeGame()
    {
        Debug.Log("Resume button clicked!");
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        scoringPanel.SetActive(true);
        //if (!isPaused || isGameOver) return;

        if (countdownRoutine != null) StopCoroutine(countdownRoutine);
        countdownRoutine = StartCoroutine(ResumeWithCountdown());
    }
    private void ResumeGameInstant()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pausePanel) pausePanel.SetActive(false);
        if (scoringPanel) scoringPanel.SetActive(true);
    }

   
    public void RestartGame()
    {
        //score = 0f;
        //isPaused = false;
        //isGameOver = false;
        //Time.timeScale = 1f;
        ResetScoreAndCoins();
        SceneManager.LoadScene("New Scene");
    }


    
    public void AdsShow()
    {
        Time.timeScale = 0f;
        if (adsPanel) adsPanel.SetActive(true);
        if (scoringPanel) scoringPanel.SetActive(false);

        SoundManager.Instance.PlaySound(soundDatabase.buttonSound);
    }

    
    private void OnRewardSuccess()
    {
        if (adsPanel) adsPanel.SetActive(false);

        isGameOver = false;
        isPaused = true; 

        if (countdownRoutine != null) StopCoroutine(countdownRoutine);
        countdownRoutine = StartCoroutine(ResumeWithCountdown());
    }

    // ------------------ Countdown ------------------
    private IEnumerator ResumeWithCountdown()
    {
        if (pausePanel) pausePanel.SetActive(false);
        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (countdownText) countdownText.gameObject.SetActive(true);

        Time.timeScale = 0f;

        int countdown = 3;
        while (countdown > 0)
        {
            if (countdownText) countdownText.text = countdown.ToString();
            yield return new WaitForSecondsRealtime(1f);  // ✅ still works while paused
            countdown--;
        }

        if (countdownText) countdownText.text = "GO!";
        yield return new WaitForSecondsRealtime(0.5f);
        if (countdownText) countdownText.gameObject.SetActive(false);

        isPaused = false;
        isGameOver = false;
        Time.timeScale = 1f;

        if (scoringPanel) scoringPanel.SetActive(true);

        SoundManager.Instance.PlaySound(soundDatabase.buttonSound);
    }



    // ------------------ Leaderboard ------------------
    private IEnumerator SubmitScorePeriodically()
    {
        while (true)
        {
            int currentScore = PlayerPrefs.GetInt("Score", 0);
            if (Leaderboards.Instance != null)
                yield return Leaderboards.Instance.SubmitScoreAsync(currentScore);

            yield return new WaitForSeconds(5f);
        }
    }

    // ------------------ Getters ------------------
    public int GetCurrentScore() => Mathf.FloorToInt(score);

    public void HomeScene()
    {
        SoundManager.Instance.PlaySound(soundDatabase.buttonSound);
        gameOverPanel.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }

    //-----------------Timer Function-----------------
    private void UpdatePlayTimeUI()
    {
        if (playTimeText == null) return;

        int minutes = Mathf.FloorToInt(playTime / 60f);
        int seconds = Mathf.FloorToInt(playTime % 60f);

        playTimeText.text = $"{minutes:00}:{seconds:00}";
        
    }
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return $"{minutes:00}:{seconds:00}";
    }


    // ---------------- Reset Score & Coin ----------------
    public void ResetScoreAndCoins()
    {
        score = 0f;
        playTime = 0f;
        isPaused = false;
        isGameOver = false;

        sessionCoins = 0; // reset session coins

        if (ScoreManager.instance != null)
            ScoreManager.instance.SetScore(0);

        if (SurvivalManager.instance != null)
            SurvivalManager.instance.ResetSurvival();

        if (tmpScoreText) tmpScoreText.text = "Score: 0";
        if (coinTextDisplay) coinTextDisplay.text = "0"; // UI reset
        if (tmpDistanceText) tmpDistanceText.text = "Score: 0";
        if (playTimeText) playTimeText.text = "00:00";

        if (scoringPanel) scoringPanel.SetActive(true);
        if (pausePanel) pausePanel.SetActive(false);
        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (adsPanel) adsPanel.SetActive(false);

        Debug.Log("✅ Score & Session Coins Reset to 0");
    }


    public void inventory()
    {
        inventoryPanel.SetActive(true);
    }
    public void inventoryback()
    {
        inventoryPanel.SetActive(false);
    }
    public void finalScoreLeaderboard()
    {
       int scoring = PlayerPrefs.GetInt("Scoring");
       print("Final Scoring uploaded to leaderboard : " + scoring);
       FirebaseSave.instance.SavePlayerToDatabase(scoring);
    }

    public void OnReviveButtonClicked()
    {
        gameOverPanel.SetActive(false);
        var player = FindObjectOfType<Chakwa.Player.PlayerRevive>();
        if (player != null)
        {
            player.RevivePlayer();
        }
    }
}
