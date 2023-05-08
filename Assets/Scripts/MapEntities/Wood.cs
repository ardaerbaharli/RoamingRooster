using Controllers;
using Enums;
using Player;
using UnityEngine;

namespace MapEntities
{
    public class Wood : MonoBehaviour
    {
        public delegate void LaneEnd(GameObject wood);

        public event LaneEnd OnFinishedLane;

        [SerializeField] private float speed;
        [SerializeField] private bool isMoving;
        [SerializeField] private Animator animator;
        public bool pastSpacing;
        private RoadDirection direction;
        private float woodSpawnPositionX;
        private float endPosition;

        private static readonly int Jump = Animator.StringToHash("Jump");


        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            if (!isMoving) return;
            transform.Translate(Vector3.forward * (Time.deltaTime * speed* GameManager.instance.gameSpeedMultiplier));

            if (direction == RoadDirection.Right)
            {
                if (!pastSpacing && transform.position.x > woodSpawnPositionX)
                    pastSpacing = true;
                else if (pastSpacing && transform.position.x > endPosition)
                    Finished();
            }
            else if (direction == RoadDirection.Left)
            {
                if (!pastSpacing && transform.position.x < woodSpawnPositionX)
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

        public void StartMoving(float woodSpawnPositionX, float endPos, RoadDirection d)
        {
            ResetValues();
            direction = d;
            isMoving = true;
            this.woodSpawnPositionX = woodSpawnPositionX;
            speed = 3f;
            endPosition = endPos;
        }

        private void ResetValues()
        {
            direction = RoadDirection.None;
            speed = 0f;
            isMoving = false;
            woodSpawnPositionX = 0f;
            endPosition = 0f;
            pastSpacing = false;
        }

        private void OnCollisionStay(Collision collisionInfo)
        {
            if (collisionInfo.collider.CompareTag("Player"))
            {
                StartFollowing(collisionInfo);
            }
        }

        private void StartFollowing(Collision collisionInfo)
        {
            var position = collisionInfo.transform.position;
            position = new Vector3(transform.position.x, position.y, position.z);
            collisionInfo.transform.position = position;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                SoundManager.instance.PlayJumpWoodSound();
                animator.SetTrigger(Jump);
                PlayerMovementManager.instance.isOnWood = true;
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.collider.CompareTag("Player"))
            {
                PlayerMovementManager.instance.isOnWood = false;
            }
        }
    }
}