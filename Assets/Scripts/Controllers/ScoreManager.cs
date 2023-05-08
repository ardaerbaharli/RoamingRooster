using UnityEngine;
using Utilities;

namespace Controllers
{
    public class ScoreManager : MonoBehaviour
    {
        public delegate void OnScoredDelegate(int score);
        public event OnScoredDelegate OnScored;
        private int score;
    
        public static ScoreManager instance;

        public int Score
        {
            get => score;
            set
            {
                score = value;
                OnScored?.Invoke(score);
                Save();
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