using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DistanceManager : MonoBehaviour
{
    public static DistanceManager instance;
    private int distanceTraveled = 0;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        LoadDistance();
    }

    void Update()
    {
        // Example: increase distance based on time or speed
        distanceTraveled += Mathf.FloorToInt(Time.deltaTime * 5); // adjust multiplier as needed
    }

    public void SaveDistance(int distance)
    {
        PlayerPrefs.SetInt("Distance", distance);
        PlayerPrefs.Save();
        Debug.Log("Distance saved: " + distance);
    }

    public void LoadDistance()
    {
        distanceTraveled = PlayerPrefs.GetInt("Distance", 0);
        Debug.Log("Distance loaded: " + distanceTraveled);
    }

    public void SetDistance(int distance)
    {
        distanceTraveled = distance;
        SaveDistance(distance);
    }

    public int GetDistance()
    {
        return distanceTraveled;
    }
}
