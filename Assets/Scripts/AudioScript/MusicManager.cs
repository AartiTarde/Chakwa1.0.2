using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    private AudioSource audioSource;


    private AudioSource loopSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = gameObject.AddComponent<AudioSource>();

            loopSource = gameObject.AddComponent<AudioSource>();
            loopSource.loop = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlaySound(AudioClip clip)
    {
        if (clip != null && PlayerPrefs.GetInt("MusicVolume", 1) == 1)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    //public void PlayLoop(AudioClip clip)
    //{
    //    if (clip != null && PlayerPrefs.GetInt("MusicVolume", 1) == 1)
    //    {
    //        if (loopSource.isPlaying && loopSource.clip == clip) return; // already playing same
    //        loopSource.clip = clip;
    //        loopSource.Play();
    //    }
    //}
    public void StopLoop()
    {
        if (loopSource.isPlaying)
            loopSource.Stop();
    }
}
