using System;
using System.Collections;
using Enums;
using Player;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class GameManager : MonoBehaviour
    {
        public delegate void GameOverEvent(GameOverType gameOverType);

        public event GameOverEvent OnGameOver;

        public delegate void GameStartEvent();

        public event GameStartEvent OnGameStarted;

        public delegate void PrepareGameEvent();

        public event PrepareGameEvent StartPreparing;

        public static GameManager instance;
        [SerializeField] private float timeRunGameTime;
        public float gameSpeedMultiplier = 1f;
        public GameType gameType;
        public float remainingTime;
        public Action<float> OnRemainingTimeChanged;

        private GameState state;

        public GameState State
        {
            get => state;
            set
            {
                PreviousState = state;
                state = value;
            }
        }

        public GameState PreviousState { get; set; }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            State = GameState.Menu;
        }

        public void PrepareGame(GameType type)
        {
            gameType = type;
            StartPreparing?.Invoke();
            PageController.Instance.ShowPage(Pages.Game);
        }

        public void StartGame()
        {
            if (gameType == GameType.TimeRun)
                StartCoroutine(StartTimeRun());
            State = GameState.Playing;
            OnGameStarted?.Invoke();
        }

        private IEnumerator StartTimeRun()
        {
            remainingTime = timeRunGameTime;
            while (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                OnRemainingTimeChanged?.Invoke(remainingTime);
                yield return null;
            }

            remainingTime = 0;
            OnRemainingTimeChanged?.Invoke(remainingTime);
            yield return null;
            GameOver(GameOverType.TimeOver);
        }

        private void Start()
        {
            ScoreManager.instance.OnScored += OnScored;
        }

        private void OnScored(int score)
        {
            MapManager.instance.GenerateTerrain(1);
            MapManager.instance.DestroyLanes(1, MapManager.Filter.First);
        }

        public void GameOver(GameOverType gameOverType, Hashtable args = null)
        {
            AdManager.Instance.GamesPlayed++;

            State = GameState.GameOver;
            switch (gameOverType)
            {
                case GameOverType.HitCar:
                    break;
                case GameOverType.FallToWater:
                    PlayerMovementManager.instance.isOnWater = true;
                    break;
                case GameOverType.FallBehind:
                    break;
                case GameOverType.HitTrain:
                    break;
                case GameOverType.TimeOver:
                    break;
            }

            PlayerMovementManager.instance.MoveForward();
            PageController.Instance.ShowPage(Pages.GameOver);
            OnGameOver?.Invoke(gameOverType);
        }

        public void Pause()
        {
            State = GameState.Paused;
            PageController.Instance.ShowPage(Pages.Pause);
        }

        public void Resume()
        {
            StartCoroutine(ResumeCoroutine());
        }

        private IEnumerator ResumeCoroutine()
        {
            PageController.Instance.ShowPage(Pages.Game);
            yield return new WaitForEndOfFrame();
            State = GameState.Playing;
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void SetRiverTrigger(bool value)
        {
            MapManager.instance.SetRiverTrigger(value);
        }


        public void SetGameState(GameState state)
        {
            State = state;
        }
    }
}