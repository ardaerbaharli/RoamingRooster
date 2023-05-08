using System.Collections;
using UnityEngine;

namespace PowerUps
{
    public abstract class PowerUp : MonoBehaviour
    {
        public PowerUpType Type;
        public float Duration;
        public abstract void Activate();

        public IEnumerator Countdown()
        {
            yield return new WaitForSeconds(Duration);
            Deactivate();
        }
        public abstract void Deactivate();
    }
}