using Controllers;
using UnityEngine;

namespace PowerUps
{
    public class FreezePowerUp : PowerUp
    {
        public float minusGameSpeedMultiplier;

        public override void Activate()
        {
            StartCoroutine(Countdown());
            GameManager.instance.gameSpeedMultiplier -= minusGameSpeedMultiplier;
        }

        public override void Deactivate()
        {
            GameManager.instance.gameSpeedMultiplier += minusGameSpeedMultiplier;
        }
    }
}