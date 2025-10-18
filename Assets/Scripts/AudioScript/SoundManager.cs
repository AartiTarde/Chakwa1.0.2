using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private AudioSource audioSource;   // one-shot SFX
    private AudioSource loopSource;    // looping SFX (e.g., run footsteps)

    private const string SoundVolumeKey = "SoundVolume";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = gameObject.AddComponent<AudioSource>();
            loopSource = gameObject.AddComponent<AudioSource>();
            loopSource.loop = true;

            // Load saved SFX volume
            float saved = PlayerPrefs.GetFloat(SoundVolumeKey, 1f);
            SetVolume(saved, save: false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Play one-shot SFX
    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }

    // Looping SFX (like running sound)
    public void PlayLoop(AudioClip clip)
    {
        if (clip != null)
        {
            if (loopSource.isPlaying && loopSource.clip == clip) return;
            loopSource.clip = clip;
            loopSource.Play();
        }
    }

    public void StopLoop()
    {
        if (loopSource.isPlaying)
            loopSource.Stop();
    }

    // 🔊 Volume control for SFX
    public void SetVolume(float value, bool save = true)
    {
        value = Mathf.Clamp01(value);
        audioSource.volume = value;
        loopSource.volume = value;

        if (save)
        {
            PlayerPrefs.SetFloat(SoundVolumeKey, value);
            PlayerPrefs.Save();
        }
    }

    public float GetVolume()
    {
        return PlayerPrefs.GetFloat(SoundVolumeKey, 1f);
    }
}
