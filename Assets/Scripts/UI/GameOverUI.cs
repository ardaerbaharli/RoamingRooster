using System;
using Controllers;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Enums;
using Player;

namespace UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI deathReasonText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI highScoreText;
        [SerializeField] private TextMeshProUGUI coinText;

        [SerializeField] private RectTransform highScoreImage;
        [SerializeField] private RectTransform scoreImage;
        [SerializeField] private RectTransform coinImage;
        [SerializeField] private RectTransform deathReasonImage;

        [SerializeField] private MessageBox messageBox;

        public static GameOverType gameOverType;

        private void OnEnable()
        {
            GameManager.instance.OnGameOver += OnGameOver;
            ScoreManager.instance.OnTopScoreChanged += OnTopScoreChanged;
        }

        private void OnTopScoreChanged(int topScore)
        {
            highScoreText.text = $"Top Score: {topScore}";
        }

        private void OnDisable()
        {
            GameManager.instance.OnGameOver -= OnGameOver;
            ScoreManager.instance.OnTopScoreChanged -= OnTopScoreChanged;
        }

        private void OnGameOver(GameOverType gameOverType)
        {
            switch (gameOverType)
            {
                case GameOverType.HitCar:
                    deathReasonText.text = "You hit a car";
                    break;
                case GameOverType.FallToWater:
                    deathReasonText.text = "You fell into water";
                    break;
                case GameOverType.FallBehind:
                    deathReasonText.text = "You fell behind";
                    break;
                case GameOverType.HitTrain:
                    deathReasonText.text = "You hit a train";
                    break;
                case GameOverType.TimeOver:
                    deathReasonText.text = "Time's up";
                    break;
            }

            highScoreText.text = $"Top Score: {ScoreManager.instance.TopScore}";
            scoreText.text = $"Score: {ScoreManager.instance.Score}";
            coinText.text = $"Coins: {CoinManager.instance.Coins}";
            ButtonMovements();
        }

        private void ButtonMovements()
        {
            highScoreImage.DOScale(1, 0.1f).SetEase(Ease.OutBack);
            scoreImage.DOScale(1, 0.2f).SetEase(Ease.OutBack).SetDelay(0.2f);
            coinImage.DOScale(1, 0.2f).SetEase(Ease.OutBack).SetDelay(0.4f);
            deathReasonImage.DOScale(1, 0.2f).SetEase(Ease.OutBack).SetDelay(0.6f);
        }

        public void RestartButton()
        {
            messageBox.Configure("Are you sure you want to restart?", "Yes", "No", () => GameManager.instance.Restart(),
                null, false);
        }


        public void StoreButton()
        {
            PageController.Instance.ShowPage(Pages.Store);
        }

        public void SettingsButton()
        {
            PageController.Instance.ShowPage(Pages.Options);
        }

        public void WatchAdToKeepPlayingButton()
        {
            AdManager.Instance.ShowAd(AdType.Rewarded);
            AdManager.Instance.OnRewardedAdClosed += () =>
            {
                GameManager.instance.Resume();
                Player.Player.Instance.IsDead = false;
            };
        }

        private void OnDestroy()
        {
            GameManager.instance.OnGameOver -= OnGameOver;
        }
    }
}