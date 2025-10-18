using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class MusicSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Awake()
    {
        if (slider == null) slider = GetComponent<Slider>();
    }

    private void Start()
    {
        float initial = BackgroundSoundManager.Instance != null
            ? BackgroundSoundManager.Instance.GetVolume()
            : PlayerPrefs.GetFloat("MusicVolume", 1f);

        slider.value = initial;

        slider.onValueChanged.AddListener(OnSliderValueChanged);

        if (BackgroundSoundManager.Instance != null)
            BackgroundSoundManager.Instance.SetVolume(initial);
    }

    public void OnSliderValueChanged(float value)
    {
        if (BackgroundSoundManager.Instance != null)
            BackgroundSoundManager.Instance.SetVolume(value);
    }
}
