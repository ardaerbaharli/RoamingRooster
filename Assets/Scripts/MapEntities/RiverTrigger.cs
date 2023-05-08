using Controllers;
using Enums;
using UnityEngine;

namespace MapEntities
{
    public class RiverTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                SoundManager.instance.PlaySplashSound();
                other.gameObject.GetComponent<Player.Player>().Die(GameOverType.FallToWater);
            }
        }
    }
}
