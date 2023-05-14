using Controllers;
using UnityEngine;

namespace UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Animator _startButtonAnimator;
        private static readonly int StartButtonAnimation = Animator.StringToHash("StartButtonAnimation");

        public void StartGameNormal()
        {
            GameManager.instance.PrepareGame(GameType.Normal);
            _startButtonAnimator.SetTrigger(StartButtonAnimation);
        }
        public void StartGameTimeRun()
        {
            GameManager.instance.PrepareGame(GameType.TimeRun);
            _startButtonAnimator.SetTrigger(StartButtonAnimation);
        }

        public void SettingsButton()
        {
            PageController.Instance.ShowPage(Pages.Options);
        }

        public void StoreButton()
        {
            PageController.Instance.ShowPage(Pages.Store);
        }
    }
}