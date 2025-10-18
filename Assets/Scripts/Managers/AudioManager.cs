using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip coinCollect;
    public AudioClip buttonClick;
    public AudioClip powerupCollect;
    public AudioClip playerHit;
    public AudioClip demonGrowl;
    public AudioClip obstacleHit;
    public AudioClip[] playerVoices;

    private bool isMusicOn = true;
    private bool isSfxOn = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAudioSettings();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        PlayMusic(backgroundMusic);
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplySettings(); 
    }
    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;

        if (isMusicOn)
            musicSource.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        if (isSfxOn && clip != null)
            sfxSource.PlayOneShot(clip);
    }

    public void PlayRandomVoice()
    {
        if (isSfxOn && playerVoices.Length > 0)
        {
            int index = Random.Range(0, playerVoices.Length);
            PlaySFX(playerVoices[index]);
        }
    }

    public void SetMusic(bool isOn)
    {
        isMusicOn = isOn;
        if (isOn)
        {
            if (!musicSource.isPlaying)
                musicSource.Play();
        }
        else
        {
            musicSource.Pause();
        }

        PlayerPrefs.SetInt("MusicOn", isOn ? 1 : 0);
    }
    public void SetSFX(bool isOn)
    {
        isSfxOn = isOn;
        PlayerPrefs.SetInt("SFXOn", isOn ? 1 : 0);
    }
    private void LoadAudioSettings()
    {
        isMusicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        isSfxOn = PlayerPrefs.GetInt("SFXOn", 1) == 1;
    }

    private void ApplySettings()
    {
        if (isMusicOn)
        {
            if (!musicSource.isPlaying && musicSource.clip != null)
                musicSource.Play();
        }
        else
        {
            musicSource.Pause();
        }
    }
}
