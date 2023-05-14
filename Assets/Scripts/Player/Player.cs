using System;
using Controllers;
using Enums;
using MapEntities;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private ParticleSystem hitCarParticles;
        [SerializeField] private ParticleSystem fallIntoWaterParticles;
        [SerializeField] private bool isCheating;
        public bool GhostMode;
        public bool ShieldMode;
        public bool IsDead;
        public static Player Instance;
        private PlayerMovementManager playerMovementManager;

        private void Awake()
        {
            Instance = this;
            playerMovementManager = GetComponent<PlayerMovementManager>();
        }

        public void SetPosition(Vector3 position,Tile currentTile)
        {
            playerMovementManager.currentTile = currentTile;
            transform.position = position;
        }

        public void Die(GameOverType type)
        {
            if (ShieldMode) return;
            if (isCheating) return;
            if (IsDead) return;
            
            IsDead = true;
            ParticleEffect(type);
            GameManager.instance.GameOver(type);
        }

        private void ParticleEffect(GameOverType gameOverType)
        {
            switch (gameOverType)
            {
                case GameOverType.HitCar:
                    hitCarParticles.Play();
                    break;
                case GameOverType.FallToWater:
                    fallIntoWaterParticles.Play();
                    break;
                case GameOverType.FallBehind:
                    break;
            }
        }
    }
}