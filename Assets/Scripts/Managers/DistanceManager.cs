using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DistanceManager : MonoBehaviour
{
    public static DistanceManager instance;

    [Header("References")]
    public Transform player;

    private Vector3 lastPosition;
    private float distanceTraveledFloat = 0f; 
    private int distanceTraveled = 0; 

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        if (player != null)
        {
            lastPosition = player.position;
        }
        LoadDistance();
    }

    void Update()
    {
        if (player != null)
        {
          
            float distanceThisFrame = Vector3.Distance(player.position, lastPosition);
            distanceTraveledFloat += distanceThisFrame;
            distanceTraveled = Mathf.FloorToInt(distanceTraveledFloat);
          
            lastPosition = player.position;
        }
    }

    public void SaveDistance()
    {
        PlayerPrefs.SetInt("Distance", distanceTraveled);
        PlayerPrefs.Save();
        Debug.Log("Distance saved: " + distanceTraveled);
    }

    public void LoadDistance()
    {
        distanceTraveled = PlayerPrefs.GetInt("Distance", 0);
        distanceTraveledFloat = distanceTraveled; 
        Debug.Log("Distance loaded: " + distanceTraveled);
        //LeaderboardScoreUploder.Instance.uploadeScore(distanceTraveled);
    }

    public void SetDistance(int distance)
    {
        distanceTraveled = distance;
        distanceTraveledFloat = distance; 
        SaveDistance();
    }

    public int GetDistance()
    {
        return distanceTraveled;
    }
}
