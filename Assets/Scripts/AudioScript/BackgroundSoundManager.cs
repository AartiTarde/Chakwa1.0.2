using UnityEngine;

public class BackgroundSoundManager : MonoBehaviour
{
    public static BackgroundSoundManager Instance;

    [SerializeField] private MusicDatabase musicDatabase;
    private AudioSource audioSource;

    private const string MusicVolumeKey = "MusicVolume";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // Default music volume
        if (!PlayerPrefs.HasKey(MusicVolumeKey))
            PlayerPrefs.SetFloat(MusicVolumeKey, 1f);

        if (musicDatabase != null && musicDatabase.music != null)
        {
            audioSource.clip = musicDatabase.music;
            audioSource.loop = true;
            audioSource.playOnAwake = false;

            float saved = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
            audioSource.volume = saved;

            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Background music not set in MusicDatabase!");
        }
    }

    public void SetVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        if (audioSource != null)
            audioSource.volume = volume;

        PlayerPrefs.SetFloat(MusicVolumeKey, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
    }

    // Extra controls
    public void StopMusic() => audioSource.Stop();
    public void PauseMusic() => audioSource.Pause();
    public void ResumeMusic() => audioSource.UnPause();
}
