using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("Player Tracking")]
    public Transform player;
    private float startZ;
    private int score; 

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        if (player != null)
            startZ = player.position.x;

        LoadDistance();
    }

    void Update()
    {
        if (player == null) return;

        float rawScore = player.position.x - startZ;
        score = Mathf.FloorToInt(Mathf.Max(0, rawScore)); // Store score as int
        Debug.Log("Player Distance: " + score + "m");
    }

    public void SaveDistance(int distance)
    {
        PlayerPrefs.SetInt("Score", distance);
        PlayerPrefs.Save();
        Debug.Log("Distance saved: " + distance);
    }

    public void LoadDistance()
    {
        score = PlayerPrefs.GetInt("Score", 0);
        Debug.Log("Distance loaded: " + score);
    }

    public void SetDistance(int distance)
    {
        score = distance;
        SaveDistance(score);
    }

    public int GetScore()
    {
        return score;
    }
}
