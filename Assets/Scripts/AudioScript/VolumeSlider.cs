using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Awake()
    {
        if (slider == null) slider = GetComponent<Slider>();
    }

    private void Start()
    {
        float initial = SoundManager.Instance != null
            ? SoundManager.Instance.GetVolume()
            : PlayerPrefs.GetFloat("SoundVolume", 1f);

        slider.value = initial;

        slider.onValueChanged.AddListener(OnSliderValueChanged);

        if (SoundManager.Instance != null)
            SoundManager.Instance.SetVolume(initial);
    }

    public void OnSliderValueChanged(float value)
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.SetVolume(value);
    }
}
