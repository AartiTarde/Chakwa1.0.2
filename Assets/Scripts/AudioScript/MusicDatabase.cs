using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MusicDatabase/Music")]
public class MusicDatabase : ScriptableObject
{
    [Header("Coin Sound")]
    public string musicName;
    public AudioClip music;
}
