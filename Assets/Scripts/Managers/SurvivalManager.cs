using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalManager : MonoBehaviour
{
    public static SurvivalManager instance;

    private float timeSurvived = 0f;      // Track survival time in seconds
    private int survivalScore = 0;        // Score calculated from time
    public int scorePerSecond = 10;       // Points earned per second

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        LoadSurvivalScore();
    }

    void Update()
    {
        // Increase survival time
        timeSurvived += Time.deltaTime;

        // Calculate score from time survived
        survivalScore = Mathf.FloorToInt(timeSurvived * scorePerSecond);
    }

    public void SaveSurvivalScore()
    {
        PlayerPrefs.SetInt("SurvivalScore", survivalScore);
        PlayerPrefs.Save();
        Debug.Log("Survival Score saved: " + survivalScore);
    }

    public void LoadSurvivalScore()
    {
        survivalScore = PlayerPrefs.GetInt("SurvivalScore", 0);
        Debug.Log("Survival Score loaded: " + survivalScore);
    }

    public void ResetSurvival()
    {
        timeSurvived = 0f;
        survivalScore = 0;
        SaveSurvivalScore();
    }

    public int GetSurvivalScore()
    {
        return survivalScore;
    }

    public float GetTimeSurvived()
    {
        return timeSurvived;
    }
}
