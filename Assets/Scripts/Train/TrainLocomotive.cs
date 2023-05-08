using Controllers;
using Enums;
using UnityEngine;

namespace Train
{
    public class TrainLocomotive : MonoBehaviour
    {
        public delegate void LaneEnd();

        public event LaneEnd OnFinishedLane;

        private float speed;
        private bool isMoving;
        private RoadDirection direction;
        private float endPosition;
        private Transform lastCarriage;

        private void FixedUpdate()
        {
            if (!isMoving) return;
            transform.Translate(Vector3.forward * (Time.deltaTime * speed * GameManager.instance.gameSpeedMultiplier));

            switch (direction)
            {
                case RoadDirection.Right:
                {
                    if (lastCarriage.position.x > endPosition)
                        Finished();
                    break;
                }
                case RoadDirection.Left:
                {
                    if (lastCarriage.position.x < endPosition)
                        Finished();
                    break;
                }
            }
        }


        private void Finished()
        {
            ResetValues();
            OnFinishedLane?.Invoke();
            gameObject.SetActive(false);
        }

        public void StartMoving(float endPos, RoadDirection d, float trainSpeed, Transform lastCarriage)
        {
            ResetValues();

            direction = d;
            isMoving = true;
            speed = trainSpeed;
            endPosition = endPos;
            this.lastCarriage = lastCarriage;
        }

        private void ResetValues()
        {
            direction = RoadDirection.None;
            speed = 0f;
            isMoving = false;
            endPosition = 0f;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<Player.Player>().Die(GameOverType.HitTrain);
            }
        }
    }
}