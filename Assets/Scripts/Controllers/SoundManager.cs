using UnityEngine;
using UnityEngine.Audio;
using Utilities;

namespace Controllers
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer mixer;

        [SerializeField] private AudioSource coinCollectSound;
        [SerializeField] private AudioSource jumpSound;
        [SerializeField] private AudioSource jumpWoodSound;
        [SerializeField] private AudioSource splashSound;
        [SerializeField] private AudioSource hitCarSound;

        public static SoundManager instance;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            SetSound(PlayerPrefsX.GetBool("Sound", true));
        }

        public void SetSound(bool value)
        {
            PlayerPrefsX.SetBool("Sound", value);
            mixer.SetFloat("Master", value ? 0 : -80);
        }

        public void PlayCoinCollectSound()
        {
            coinCollectSound.Play();
        }

        public void PlayJumpSound()
        {
            jumpSound.Play();
        }

        public void PlayJumpWoodSound()
        {
            jumpWoodSound.Play();
        }

        public void PlaySplashSound()
        {
            splashSound.Play();
        }

        public void PlayHitCarSound()
        {
            hitCarSound.Play();
        }
    }
}