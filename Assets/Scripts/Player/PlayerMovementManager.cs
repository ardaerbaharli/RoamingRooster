using System.Collections;
using System.Collections.Generic;
using Controllers;
using Enums;
using MapEntities;
using ScriptableObjects;
using UnityEngine;

namespace Player
{
    public class PlayerMovementManager : MonoBehaviour
    {
        public delegate void OnMovedDelegate(Vector3 position);

        public delegate void OnStartedMovingDelegate();

        public delegate void OnJumpedOnWoodDelegate();

        public delegate void OnJumpedOffWoodDelegate();

        public event OnMovedDelegate OnMoved;
        public event OnStartedMovingDelegate OnStartedMoving;
        public event OnJumpedOnWoodDelegate OnJumpedOnWood;
        public event OnJumpedOffWoodDelegate OnJumpedOffWood;

        public Tile currentTile;
        private bool isMoving;
        private Vector3 centerPoint;
        private float journeyTime;
        private float jumpHeight;
        private PlayerMovementSettings settings;
        private Queue<Move> moves;
        public static PlayerMovementManager instance;
        private bool _isOnWood;

        public bool isOnWood
        {
            get => _isOnWood;
            set
            {
                _isOnWood = value;
                if (value)
                    OnJumpedOnWood?.Invoke();
                else
                    OnJumpedOffWood?.Invoke();
            }
        }

        private void Awake()
        {
            settings = Resources.Load<PlayerMovementSettings>("PlayerMovementSettings");
            journeyTime = settings.journeyTime;
            jumpHeight = settings.jumpHeight;

            instance = this;
        }

        private void Start()
        {
            moves = new Queue<Move>();
            SwipeManager.Instance.OnTap += OnTap;
            SwipeManager.Instance.OnSwipeLeft += OnSwipeLeft;
            SwipeManager.Instance.OnSwipeRight += OnSwipeRight;
            SwipeManager.Instance.OnSwipeUp += OnSwipeUp;
            SwipeManager.Instance.OnSwipeDown += OnSwipeDown;
        }


        private void OnDestroy()
        {
            SwipeManager.Instance.OnTap -= OnTap;
            SwipeManager.Instance.OnSwipeLeft -= OnSwipeLeft;
            SwipeManager.Instance.OnSwipeRight -= OnSwipeRight;
            SwipeManager.Instance.OnSwipeUp -= OnSwipeUp;
            SwipeManager.Instance.OnSwipeDown -= OnSwipeDown;
        }

        private void OnSwipeDown() => Move(Direction.Down);

        private void OnSwipeUp() => Move(Direction.Up);

        private void OnSwipeRight() => Move(Direction.Right);

        private void OnSwipeLeft() => Move(Direction.Left);

        private void OnTap() => Move(Direction.Up);

        private void Move(Direction d)
        {
            if (moves.Count > 1) return;
            var nextLane = d switch
            {
                Direction.Up => currentTile.lane.nextLane,
                Direction.Down => currentTile.lane.previousLane,
                _ => currentTile.lane
            };

            if (nextLane == null) return;

            int nextTileIndex;
            if (isOnWood)
            {
                var laneIndex = currentTile.lane.laneIndex;
                nextTileIndex = MapManager.instance.GetTile(laneIndex, transform.position.x).tileIndex;
            }
            else
            {
                nextTileIndex = currentTile.tileIndex;
                var addition = d switch
                {
                    Direction.Right => 1,
                    Direction.Left => -1,
                    _ => 0
                };
                nextTileIndex += addition;
            }


            if (nextTileIndex < 0 || nextTileIndex >= nextLane.tileObjects.Count) return;
            var nextTileObj = nextLane.tileObjects[nextTileIndex];
            var nextTile = nextTileObj.GetComponent<Tile>();
            if (!Player.Instance.GhostMode)
                if (nextTile.hasObstacle)
                    return;

            var angle = d switch
            {
                Direction.Up => new Vector3(0, 0, 0),
                Direction.Down => new Vector3(0, 180, 0),
                Direction.Left => new Vector3(0, 270, 0),
                Direction.Right => new Vector3(0, 90, 0),
            };

            var move = new Move
            {
                angle = angle,
                nextTile = nextTile,
                targetPos = nextTile.transform.position
            };
            currentTile = move.nextTile;

            if (currentTile.lane.laneIndex + 1 > ScoreManager.instance.Score)
                ScoreManager.instance.Score = currentTile.lane.laneIndex + 1;

            moves.Enqueue(move);
        }

        private void Update()
        {
            if (isMoving) return;
            if (moves.Count == 0) return;
            var move = moves.Dequeue();
            StartCoroutine(MoveTo(move));
            StartCoroutine(RotateCharacter(move.angle));
        }

        private IEnumerator MoveTo(Move move)
        {
            isMoving = true;
            OnStartedMoving?.Invoke();
            var startPos = transform.position;
            var endPos = new Vector3(move.targetPos.x, startPos.y, move.targetPos.z);
            var center = (endPos + startPos) / 2;
            var peak = new Vector3(center.x, center.y - jumpHeight,
                center.z);

            var startRelativeCenter = startPos - peak;
            var endRelativeCenter = endPos - peak;

            var t = 0.0f;
            while (t < journeyTime)
            {
                t += Time.deltaTime;
                transform.position = Vector3.Slerp(startRelativeCenter, endRelativeCenter, t / journeyTime) + peak;

                yield return null;
            }

            if (!isOnWood)
                SoundManager.instance.PlayJumpSound();
            OnMoved?.Invoke(transform.position);
            isMoving = false;
        }


        private IEnumerator RotateCharacter(Vector3 targetRotation)
        {
            var startRotation = transform.rotation.eulerAngles;
            var t = 0.0f;
            while (t < journeyTime)
            {
                t += Time.deltaTime;
                transform.rotation = Quaternion.Euler(Vector3.Lerp(startRotation, targetRotation, t / journeyTime));
                yield return null;
            }
        }
    }
}