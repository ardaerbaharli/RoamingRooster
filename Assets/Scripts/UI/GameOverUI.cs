using Controllers;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Enums;

namespace UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI deathReasonText;

        [SerializeField] private RectTransform highScoreImage;
        [SerializeField] private RectTransform scoreImage;
        [SerializeField] private RectTransform coinImage;
        [SerializeField] private RectTransform deathReasonImage;

        private void Start()
        {
            GameManager.instance.OnGameOver += OnGameOver;
        }

        private void OnGameOver(GameOverType gameOverType)
        {
            // deathReasonText.GetComponent<LocalizeStringEvent>().SetEntry(gameOverType.ToString());
            ButtonMovements();
        }

        private void ButtonMovements()
        {
            highScoreImage.DOLocalMoveZ(0, 1f).SetEase(Ease.Linear);
            scoreImage.DOLocalMoveZ(0, 1f).SetEase(Ease.Linear).SetDelay(0.5f);
            coinImage.DOLocalMoveZ(0, 1f).SetEase(Ease.Linear).SetDelay(1f);
            deathReasonImage.DOLocalMoveZ(0, 1f).SetEase(Ease.Linear).SetDelay(1.5f);
        }

        public void RestartButton()
        {
            GameManager.instance.Restart();
        }

        public void StoreButton()
        {
            PageController.Instance.ShowPage(Pages.Store);
        }

        public void SettingsButton()
        {
            PageController.Instance.ShowPage(Pages.Options);
        }

        private void OnDestroy()
        {
            GameManager.instance.OnGameOver -= OnGameOver;
        }
    }
}