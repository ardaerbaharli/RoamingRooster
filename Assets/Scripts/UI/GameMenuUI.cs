using System;
using System.Collections;
using System.Net;
using Controllers;
using PowerUps;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameMenuUI : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI coinText;

        [SerializeField] private Animator scoreAnimator;
        [SerializeField] private Animator coinAnimator;

        private static readonly int Animation = Animator.StringToHash("StartAnimation");

        private bool gameOver;


        [SerializeField] private Image freezePowerUpImage, ghostPowerUpImage, shieldPowerUpImage;
        private Button freezePowerUpButton, ghostPowerUpButton, shieldPowerUpButton;
        [SerializeField] private TextMeshProUGUI remainingTimeText;


        private void Awake()
        {
            freezePowerUpButton = freezePowerUpImage.transform.parent.GetComponent<Button>();
            ghostPowerUpButton = ghostPowerUpImage.transform.parent.GetComponent<Button>();
            shieldPowerUpButton = shieldPowerUpImage.transform.parent.GetComponent<Button>();
            if (GameManager.instance.gameType == GameType.TimeRun)
            {
                GameManager.instance.OnRemainingTimeChanged += OnRemainingTimeChanged;
                remainingTimeText.gameObject.SetActive(true);
            }
        }

        private void OnRemainingTimeChanged(float time)
        {
            remainingTimeText.text = time.ToString("F1");
        }

        private void Start()
        {
            ScoreManager.instance.OnScored += OnScored;
            CoinManager.instance.OnCoinGained += OnCoinGained;
        }

        private void OnScored(int score)
        {
            scoreText.text = score.ToString();
            if (score % 5 == 0)
            {
                scoreAnimator.SetTrigger(Animation);
            }
        }

        private void OnCoinGained(int coin)
        {
            coinText.text = coin.ToString();
            coinAnimator.SetTrigger(Animation);
        }

        public void PauseButton()
        {
            GameManager.instance.Pause();
        }

        public void FreezePowerUpButton()
        {
            var type = PowerUpType.Freeze;
            PowerUpButton(type);
        }

        public void GhostPowerUpButton()
        {
            var type = PowerUpType.Ghost;
            PowerUpButton(type);
        }

        public void ShieldPowerUpButton()
        {
            var type = PowerUpType.Shield;
            PowerUpButton(type);
        }

        private void PowerUpButton(PowerUpType type)
        {
            if (PowerUpManager.Instance.IsInCooldown(type)) return;
            var result = PowerUpManager.Instance.UsePowerUp(type);
            if (result) StartCoroutine(CooldownCoroutine(type));
        }

        private IEnumerator CooldownCoroutine(PowerUpType type)
        {
            var button = GetButton(type);
            button.interactable = false;
            var image = GetImage(type);
            image.fillAmount = 1f;
            var powerUpDuration = PowerUpManager.Instance.GetPowerUpDuration(type);
            yield return new WaitForSeconds(powerUpDuration);

            // fill amount to 0 in PowerUpManager.Instance.cooldownTime seconds
            var fillAmount = 1f;
            var time = 0f;
            while (fillAmount > 0)
            {
                time += Time.deltaTime;
                fillAmount = Mathf.Lerp(1, 0, time / PowerUpManager.Instance.cooldownTime);
                image.fillAmount = fillAmount;
                yield return null;
            }

            button.interactable = true;
            PowerUpManager.Instance.SetCoolDown(type, false);
        }

        private Button GetButton(PowerUpType type)
        {
            return type switch
            {
                PowerUpType.Freeze => freezePowerUpButton,
                PowerUpType.Ghost => ghostPowerUpButton,
                PowerUpType.Shield => shieldPowerUpButton,
                _ => freezePowerUpButton
            };
        }

        private Image GetImage(PowerUpType type)
        {
            return type switch
            {
                PowerUpType.Freeze => freezePowerUpImage,
                PowerUpType.Ghost => ghostPowerUpImage,
                PowerUpType.Shield => shieldPowerUpImage,
                _ => freezePowerUpImage
            };
        }
    }
}