using Controllers;
using Enums;
using UnityEngine;

namespace MapEntities
{
    public class Car : MonoBehaviour
    {
        public delegate void LaneEnd(GameObject car);

        public event LaneEnd OnFinishedLane;

        [SerializeField] private float speed;
        [SerializeField] private bool isMoving;
        private RoadDirection direction;
        public bool pastSpacing;
        private float carSpawnPositionX;
        private float endPosition;

        private void FixedUpdate()
        {
            if (!isMoving) return;
            transform.Translate(Vector3.forward * (Time.deltaTime * speed * GameManager.instance.gameSpeedMultiplier));

            if (direction == RoadDirection.Right)
            {
                if (!pastSpacing && transform.position.x > carSpawnPositionX)
                    pastSpacing = true;
                else if (pastSpacing && transform.position.x > endPosition)
                    Finished();
            }
            else if (direction == RoadDirection.Left)
            {
                if (!pastSpacing && transform.position.x < carSpawnPositionX)
                    pastSpacing = true;
                else if (pastSpacing && transform.position.x < endPosition)
                    Finished();
            }
        }


        private void Finished()
        {
            ResetValues();
            OnFinishedLane?.Invoke(gameObject);
            gameObject.SetActive(false);
        }

        public void StartMoving(float carSpawnPositionX, float endPos, RoadDirection d, float speed)
        {
            ResetValues();
            direction = d;
            isMoving = true;
            this.carSpawnPositionX = carSpawnPositionX;
            this.speed = speed;
            endPosition = endPos;
        }

        private void ResetValues()
        {
            direction = RoadDirection.None;
            speed = 0f;
            isMoving = false;
            carSpawnPositionX = 0f;
            endPosition = 0f;
            pastSpacing = false;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                SoundManager.instance.PlayHitCarSound();
                other.gameObject.GetComponent<Player.Player>().Die(GameOverType.HitCar);
            }
        }
    }
}