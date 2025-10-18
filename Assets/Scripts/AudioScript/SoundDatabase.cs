using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SoundDatabase/Sounds")]
public class SoundDatabase : ScriptableObject
{
    //coin 
    [Header("Coin Sound")]
    public string coinSoundName;
    public AudioClip coinSound;

    //Magnet
    [Header("Magnet Sound")]
    public string magnet;
    public AudioClip magnetSound;


    //BoostUps
    [Header("BoostUps Sound")]
    public string boostUps;
    public AudioClip boostUpsSound;

    //BoostUps
    [Header("Thriller Sound")]
    public string thriller;
    public AudioClip thrillerSound;

    //BoostUps
    [Header("Run Sound")]
    public string run;
    public AudioClip runSound;

    //BoostUps
    [Header("Run Sounds")]
    public string runs;
    public AudioClip runSounds;

    //PlayerRunningSound
    [Header("Turn Sound")]
    public string turnSound;
    public AudioClip playerturnSound;

    //PlayerRunningSound
    [Header("ButtonSound")]
    public string btnSound;
    public AudioClip buttonSound;

    [Header("Jump Sound")]
    public string jumpSound;
    public AudioClip jumpplayerSound;

    [Header("OnGround Sound")]
    public string onGroundSound;
    public AudioClip onGroundplayerSound;

    [Header("DemonLaugh")]
    public string demonLaugh;
    public AudioClip demonLaughSound;

    [Header("DemonLaugh")]
    public string specialCoin;
    public AudioClip specialCoins;
    //[Header("BackgroundSoundTrack")]
    //public string bgSounds;
    //public AudioClip bgSound;
}
