using UnityEngine;
using Utilities;

namespace Controllers
{
    public class CoinManager : MonoBehaviour
    {
        public delegate void OnCoinGainedDelegate(int coin);

        public event OnCoinGainedDelegate OnCoinGained;
        private int _coin;
        

        public int Coins
        {
            get => _coin;
            set
            {
                _coin = value;
                OnCoinGained?.Invoke(_coin);
                Save();
            }
        }

        public static CoinManager instance;

        private void Awake()
        {
            instance = this;
            Coins = PlayerPrefs.GetInt(Config.CoinPref, 0);
        }

        private void Save()
        {
            PlayerPrefs.SetInt(Config.CoinPref, Coins);
        }
        
        public void GainCoins(int amount)
        {
            Coins += amount;
        }

        public void LoseCoins(int amount)
        {
            Coins -= amount;
        }
    }
}