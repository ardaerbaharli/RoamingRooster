using Controllers;
using DG.Tweening;
using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        [SerializeField] private ToggleSwitch soundToggle;
        [SerializeField] private ToggleSwitch vibrationToggle;
        
        [SerializeField] private MessageBox messageBox;

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

        public void ResumeButton()
        {
            GameManager.instance.Resume();
        }
        public void RestartButton()
        {
            messageBox.Configure("Are you sure you want to restart?", "Yes", "No",RestartGame, null);
        }
        private void RestartGame()
        {
            GameManager.instance.Restart();
        }
    }
}