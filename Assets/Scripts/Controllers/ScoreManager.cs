using UnityEngine;
using Utilities;

namespace Controllers
{
    public class ScoreManager : MonoBehaviour
    {
        public delegate void OnScoredDelegate(int score);
        public event OnScoredDelegate OnScored;
        public event OnScoredDelegate OnTopScoreChanged;
        private int score;
    
        public static ScoreManager instance;

        public int Score
        {
            get => score;
            set
            {
                score = value;
                if (score > TopScore)
                {
                    TopScore = score;
                }
                OnScored?.Invoke(score);
                Save();
            }
        }
        public int TopScore
        {
            get => PlayerPrefs.GetInt(Config.TopScorePref, 0);
            set
            {
                PlayerPrefs.SetInt(Config.TopScorePref, value);
                OnTopScoreChanged?.Invoke(value);
            }
        }
        
        private void Awake()
        {
                instance = this;
        }

        private void Save()
        {
            PlayerPrefs.SetInt(Config.ScorePref, Score);
        }
    }
}