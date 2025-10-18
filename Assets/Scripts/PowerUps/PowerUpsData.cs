using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (menuName = "PowerUpsData/PowerUps")]
public class PowerUpsData : ScriptableObject
{
    //coin powerups
    [Header("CoinPowerUps")]
    public string coinpowerups;
    public float coinpowerupTime;

    //run powerups
    [Header("RunPowerUps")]
    public string runpowerups;
    public float runpowerupTime;
    public float speed;
}
