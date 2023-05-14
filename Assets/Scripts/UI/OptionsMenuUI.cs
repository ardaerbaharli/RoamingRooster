using Controllers;
using Enums;
using UnityEngine;
using Utilities;

namespace UI
{
    public class OptionsMenuUI : MonoBehaviour
    {
        [SerializeField] private ToggleSwitch soundToggle;
        [SerializeField] private ToggleSwitch vibrationToggle;

        private void Start()
        {
            soundToggle.Toggle(Config.IsSoundOn);
            vibrationToggle.Toggle(Config.IsVibrationOn);
        }

        public void OnVibrationToggleValueChanged(bool value)
        {
            //TODO: Set vibration value
            Config.IsVibrationOn = value;
        }

        public void OnSoundToggleValueChanged(bool value)
        {
            SoundManager.instance.SetSound(value);
            Config.IsSoundOn = value;
        }

        public void BackButton()
        {
            GameManager.instance.State = GameState.Playing;
            PageController.Instance.GoBack();
        }
    }
}