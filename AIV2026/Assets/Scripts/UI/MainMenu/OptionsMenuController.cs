using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuController : MonoBehaviour
{
    [SerializeField] private Slider slider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider.value = AudioManager.Instance.GetMasterVolume();
        slider.onValueChanged.AddListener(OnVolumeChanged);
    }
    private void OnVolumeChanged(float value)
    {
        AudioManager.Instance.SetMasterVolume(value);
    }

    // Update is called once per frame
    void Update()
    {

    }
}