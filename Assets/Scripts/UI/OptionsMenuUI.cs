using System;
using Controllers;
using Enums;
using UnityEngine;
using UnityEngine.Audio;
using Utilities;

namespace UI
{
    public class OptionsMenuUI : MonoBehaviour
    {
        [SerializeField] private ToggleSwitch soundToggle;
        [SerializeField] private ToggleSwitch vibrationToggle;
        [SerializeField] private DoubleSwitch languageToggle;

        private void Start()
        {
            languageToggle.SetSwitchNames("tr", "en");
            languageToggle.ActivateSwitch(Config.ActiveLanguage);
            soundToggle.Toggle(Config.IsSoundOn);
            vibrationToggle.Toggle(Config.IsVibrationOn);

            languageToggle.OnSwitchChanged += OnLanguageToggleValueChanged;
        }

        private void OnLanguageToggleValueChanged(string s)
        {
            if (Config.ActiveLanguage.Equals(s)) return;
            Config.ActiveLanguage = s;
        }

        private void OnDestroy()
        {
            languageToggle.OnSwitchChanged -= OnLanguageToggleValueChanged;
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