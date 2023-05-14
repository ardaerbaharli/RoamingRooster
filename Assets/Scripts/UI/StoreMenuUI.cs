using Controllers;
using PowerUps;
using UnityEngine;

namespace UI
{
    public class StoreMenuUI : MonoBehaviour
    {
        [SerializeField] private MessageBox messageBox;
        [SerializeField] private int buyCoinsAmount;
        [SerializeField] private int powerUpPrice;

        public void BackButton()
        {
            PageController.Instance.GoBack();
        }

        public void BuyCoinsButton()
        {
            messageBox.Configure("Are you sure you want to buy 1000 coins?", "Yes", "No", IapManager.Instance.BuyCoins(buyCoinsAmount), null);
        }

        public void BuyRemoveAdsButton()
        {
            messageBox.Configure("Are you sure you want to buy remove ads?", "Yes", "No", IapManager.Instance.BuyRemoveAds(), null);
        }

        public void BuyShieldPowerUpButton()
        {
            messageBox.Configure("Are you sure you want to buy shield power up?", "Yes", "No", BuyShieldPowerUp, null);

            void BuyShieldPowerUp()
            {
                var success = CoinManager.instance.Coins >= powerUpPrice;
                print("Bought shield power up: " + success);
                if (!success) return;
                CoinManager.instance.Coins -= powerUpPrice;
                PowerUpManager.Instance.AddPowerUp(PowerUpType.Shield);
            }
        }

        public void BuyDoubleJumpPowerUpButton()
        {
            messageBox.Configure("Are you sure you want to buy double jump power up?", "Yes", "No",
                BuyDoubleJumpPowerUp, null);

            void BuyDoubleJumpPowerUp()
            {
                var success = CoinManager.instance.Coins >= powerUpPrice;
                print("Bought double jump power up: " + success);
                if (!success) return;
                CoinManager.instance.Coins -= powerUpPrice;
                PowerUpManager.Instance.AddPowerUp(PowerUpType.Freeze);
            }
        }

        public void BuyGhostPowerUpButton()
        {
            messageBox.Configure("Are you sure you want to buy ghost power up?", "Yes", "No", BuyGhostPowerUp, null);

            void BuyGhostPowerUp()
            {
                var success = CoinManager.instance.Coins >= powerUpPrice;
                print("Bought ghost power up: " + success);
                if (!success) return;
                CoinManager.instance.Coins -= powerUpPrice;
                PowerUpManager.Instance.AddPowerUp(PowerUpType.Ghost);
            }
        }
    }
}