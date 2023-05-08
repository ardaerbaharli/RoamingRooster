using System;
using Controllers;
using Enums;
using ScriptableObjects;
using UI;
using UnityEngine;
using Utilities;

namespace Player
{
    public class CameraMovementManager : MonoBehaviour
    {
        [SerializeField] private PlayerMovementManager playerMovementManager;
        [SerializeField] private bool passiveMove;

        private Animator _animator;
        private CameraSettings cameraSettings;
        private Vector3 offset;
        private float passiveSpeed;
        private float followSpeed;
        private float cameraThreshold;
        private Vector3 playerLastPos;
        private bool isFollowing;
        private bool isSet;
        private static readonly int RotateToGame = Animator.StringToHash("RotateToGame");

        private void Awake()
        {
            cameraSettings = Resources.Load<CameraSettings>("CameraSettings");
            _animator = GetComponent<Animator>();
        }

        private void Set()
        {
            offset = cameraSettings.offset;
            passiveSpeed = cameraSettings.passiveSpeed;
            followSpeed = cameraSettings.followSpeed;
            cameraThreshold = cameraSettings.cameraThreshold;
            passiveMove = cameraSettings.passiveMove;
        }

        private void StartPreparing()
        {
            _animator.SetTrigger(RotateToGame);
        }

        private void RotationComplete()
        {
            Set();
            playerLastPos = playerMovementManager.transform.position;
            transform.position = playerLastPos + offset;
            isSet = true;
            GameManager.instance.StartGame();
        }

        private void Start()
        {
            GameManager.instance.StartPreparing += StartPreparing;
            playerMovementManager.OnStartedMoving += OnStartedMoving;
        }

        private void OnStartedMoving()
        {
            isFollowing = true;
        }

        private void Update()
        {
            if (!isSet) return;
            if (GameManager.instance.State != GameState.Playing) return;

            if (passiveMove)
                transform.position += Vector3.forward * (passiveSpeed * Time.deltaTime);

            playerLastPos = playerMovementManager.transform.position;

            if (playerLastPos.z - transform.position.z < cameraThreshold)
                GameManager.instance.GameOver(GameOverType.FallBehind,
                    Helpers.Hash("playerDeathPosition", playerLastPos));

            if (isFollowing)
            {
                var targetPos = playerLastPos + offset;

                if (targetPos.z < transform.position.z) return;
                transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
            }
        }
    }
}