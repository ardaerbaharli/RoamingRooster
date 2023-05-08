using Enums;
using UnityEngine;

namespace Train
{
    public class TrainCarriage : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<Player.Player>().Die(GameOverType.HitTrain);
            }
        }
    }
}