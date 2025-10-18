using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Panels")]
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public GameObject transitionScreen;
    public GameObject ScoringPanel;
    public GameObject AdsPanel;

    [Header("Score UI")]    
    public TMP_Text tmpScoreText;   
    public TMP_Text scoreText;
    public TMP_Text tmpDistanceText;

    [Header("Game State")]
    private float score = 0f;
    private bool isPaused = false;
    private bool isGameOver = false;

    [Header("Player Tracking")]
    public Transform player;
    private float startZ;

    [Header("GameOverPanel")]
    public TMP_Text textscore;
    public TMP_Text distanceText;
    public TMP_Text scoreTravelText;

    [Header("paused")]
    public TMP_Text pscore;
    public TMP_Text pdistanceText;
    public TMP_Text coin;


   
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player")?.transform;

        if (player != null)
            startZ = player.position.x;

        UpdateCoinUI(); //updateCoinUI()
        ResumeGame();  //ResumeGame()
        HideAllPanels(); //HideAllPanels Function()
        UpdateDistanceUI(); //UpdateDistanceUI
        StartCoroutine(SubmitScorePeriodically());
    }
    public void UpdateCoinUI() //UpdateCoinUI
    {
        if (scoreText != null && CoinManager.instance != null)
        {
            scoreText.text = "" + CoinManager.instance.GetCoinCount();
        }
    }

    void Update()
    {
       
        if (!isPaused && !isGameOver)
        {
            score += Time.deltaTime * 10f;
            UpdateScoreUI();
            UpdateDistanceUI();
        }

       
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }
    private void HideAllPanels()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (transitionScreen != null)
            transitionScreen.SetActive(false);

        if(ScoringPanel!=null)
            ScoringPanel.SetActive(true);

        if (AdsPanel != null)
            AdsPanel.SetActive(false);

    }

    private void UpdateScoreUI()
    {
        int currentScore = Mathf.FloorToInt(score);
        if (tmpScoreText != null)
            tmpScoreText.text = "Score: " + currentScore;
    }
    private void UpdateDistanceUI()
    {
        if (tmpDistanceText != null && player != null)
        {
            float distance = player.position.x - startZ;
            int roundedDistance = Mathf.FloorToInt(Mathf.Max(0, distance)); // prevent negatives
           
            print("GameManager leaderboard UI :" + roundedDistance);//tmpDistanceText.text = "Distance: " + roundedDistance + "m";
            PlayerPrefs.SetInt("Score", roundedDistance);
            PlayerPrefs.Save();
            print("Player Distance : "+ roundedDistance+"m");

            
        }
    }
    private IEnumerator SubmitScorePeriodically()
    {
        while (true)
        {
            int currentScore = PlayerPrefs.GetInt("Score", 0);
            print("Score leaderboards :"+currentScore);
            yield return Leaderboards.Instance.SubmitScoreAsync(currentScore);
            yield return new WaitForSeconds(5f); // adjust interval as needed
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        isGameOver = true;
        isPaused = true;
        Time.timeScale = 0f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
            ScoringPanel.SetActive(false);
            AdsPanel.SetActive(false);
            if (textscore != null && CoinManager.instance != null)
            {
                textscore.text = "" + CoinManager.instance.GetCoinCount();
                long finalScore = ScoreManager.instance.GetScore();     
                if (Leaderboards.Instance != null)
                {
                    Debug.Log("Leaderboards :"+finalScore);
                    Leaderboards.Instance.SubmitScoreAsync(finalScore);
                }
                else
                {
                    Debug.LogWarning("Leaderboard instance not found!");
                }
            distanceText.text = "" + DistanceManager.instance.GetDistance();
                scoreTravelText.text=""+ScoreManager.instance.GetScore();
            }

    }
  
    public void PauseGame()
    {
        if (isPaused) return;

        isPaused = true;
        Time.timeScale = 0f;

        if (pausePanel != null)
            pausePanel.SetActive(true);
            ScoringPanel.SetActive(false);
            if (pscore != null && CoinManager.instance != null)
            {
                pscore.text = "" + ScoreManager.instance.GetScore();
                pdistanceText.text = "" + DistanceManager.instance.GetDistance();
                coin.text = "" + CoinManager.instance.GetCoinCount(); ;
            }
    }
    public void ResumeGame()
    {
        if (!isPaused) return;

        isPaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);
            ScoringPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Debug.Log("Restarting game...");

        if (transitionScreen != null)
            transitionScreen.SetActive(true); 

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (pausePanel != null)
            pausePanel.SetActive(false);

        score = 0f;
        isPaused = false;
        isGameOver = false;
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public int GetCurrentScore()
    {
        return Mathf.FloorToInt(score);
    }

    public void AdsShow()
    {
        Time.timeScale = 0f;
        AdsPanel.SetActive(true);
    }
  
    public void homeScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void showAds()
    {
        Time.timeScale = 1f;

        PlayerMovement.Instance.RespawnPlayer();
        PlayerMovement.Instance.playerControl();

        AdsPanel.SetActive(false);

        Debug.Log("Ad finished. Player respawned.");
    }

}
