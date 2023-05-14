using System;
using UnityEngine;

namespace Controllers
{
    public class IapManager : MonoBehaviour
    {
        public static IapManager Instance;
        private bool success;

        private void Awake()
        {
            Instance = this;
            success = true;
        }
        public Action BuyCoins(int buyCoinsAmount)
        {
            if (success)
            {
                CoinManager.instance.Coins += buyCoinsAmount;
            }
            return null;
        }
        public Action BuyRemoveAds()
        {
            if (success)
            {
                 AdManager.Instance.RemoveAds();
            }
            return null;
        }
    }
}
