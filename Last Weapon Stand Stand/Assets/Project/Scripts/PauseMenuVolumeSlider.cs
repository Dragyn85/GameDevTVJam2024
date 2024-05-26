using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenuVolumeSlider : MonoBehaviour
{
    [SerializeField] private string _soundParameterName;
    [SerializeField] AudioMixer _mixer;
    [SerializeField] Slider _slider;

    private void Start()
    {
        _slider.value = AudioManager.Instance.GetSoundNormalizedParameterValue(_soundParameterName);
        
        _slider.onValueChanged.AddListener(SliderChanged);
    }

    public void SliderChanged(float value)
    {
        AudioManager.Instance.SetVolume(_soundParameterName, value * 100);
    }
}