using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PowerUps
{
    public class PowerUpManager : MonoBehaviour
    {
        public static PowerUpManager Instance;
        private Dictionary<PowerUpType, int> powerUpInventory;
        private Dictionary<PowerUpType, bool> powerUpCooldowns;
        private Dictionary<PowerUpType, PowerUp> powerUps;
        public float cooldownTime = 5f;

        private PowerUp ghostPowerUp, freezePowerUp, shieldPowerUp;

        private void Awake()
        {
            Instance = this;
            ghostPowerUp = GetComponent<GhostPowerUp>();
            freezePowerUp = GetComponent<FreezePowerUp>();
            shieldPowerUp = GetComponent<ShieldPowerUp>();
            powerUps = new Dictionary<PowerUpType, PowerUp>
            {
                {PowerUpType.Ghost, ghostPowerUp},
                {PowerUpType.Freeze, freezePowerUp},
                {PowerUpType.Shield, shieldPowerUp}
            };
            
            powerUpCooldowns = new Dictionary<PowerUpType, bool>
            {
                {PowerUpType.Ghost, false},
                {PowerUpType.Freeze, false},
                {PowerUpType.Shield, false}
            };

            SetupInventory();
        }

        private void SetupInventory()
        {
            powerUpInventory = new Dictionary<PowerUpType, int>();
            foreach (PowerUpType powerUpType in Enum.GetValues(typeof(PowerUpType)))
            {
                var powerUpName = powerUpType.ToString();
                var powerUpCount = PlayerPrefs.GetInt(powerUpName, 10);
                powerUpInventory.Add(powerUpType, powerUpCount);
            }
        }

        public bool UsePowerUp(PowerUpType type)
        {
            if (!HasPowerUp(type)) return false;

            powerUpInventory[type]--;
            PlayerPrefs.SetInt(type.ToString(), powerUpInventory[type]);
            powerUps[type].Activate();
            SetCoolDown(type,true);
            return true;
        }

        public void AddPowerUp(PowerUpType type)
        {
            powerUpInventory[type]++;
            PlayerPrefs.SetInt(type.ToString(), powerUpInventory[type]);
        }

        public bool HasPowerUp(PowerUpType type)
        {
            return powerUpInventory[type] > 0;
        }

        public void SetCoolDown(PowerUpType type,bool value)
        {
            powerUpCooldowns[type] = value;
        }

        public bool IsInCooldown(PowerUpType type)
        {
            return powerUpCooldowns[type];
        }

        public float GetPowerUpDuration(PowerUpType type)
        {
            return powerUps[type].Duration;
        }
    }
}