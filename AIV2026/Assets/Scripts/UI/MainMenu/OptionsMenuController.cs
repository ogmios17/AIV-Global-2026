using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuController : MonoBehaviour
{
    [SerializeField] private Slider slider;


//   void Awake()
//   {
//       if (AudioManager.Instance != null)
//       {
//           slider.value = AudioManager.Instance.GetMasterVolume();
//           slider.onValueChanged.AddListener(OnVolumeChanged);
//       }
//   }
//   void Start()
//   {
//       slider.value = AudioManager.Instance.GetMasterVolume();
//       slider.onValueChanged.AddListener(OnVolumeChanged);
//   }
//   private void OnVolumeChanged(float value)
//   {
//       AudioManager.Instance.SetMasterVolume(value);
//   }
//
//   // Update is called once per frame
//   void Update()
//   {
//
//   }
}