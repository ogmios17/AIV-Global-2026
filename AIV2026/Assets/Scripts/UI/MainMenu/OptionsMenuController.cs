using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuController : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    public void OnVolumeSliderChanged(float value)
    {
        // Handle volume change logic here
    }
}
