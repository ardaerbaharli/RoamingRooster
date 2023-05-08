using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using MapEntities;
using Unity.VisualScripting;
using UnityEngine;
using Utilities;
using static UnityEngine.Random;

namespace Spawners
{
    public class CoinSpawner : MonoBehaviour
    {
        [SerializeField] private int everyXLanes;
        [SerializeField] private int startAfterXLanes;
        private float leftBorder;
        private float rightBorder;
        private float width => rightBorder - leftBorder;
        private int lastSpawnedIndex;
        private int numberOfTiles;
        private float tileWidth;
        private List<PooledObject> coins;


        public void StartSpawning(int numberOfTiles, float leftBorder, float rightBorder)
        {
            coins = new List<PooledObject>();

            this.numberOfTiles = numberOfTiles;
            this.leftBorder = leftBorder;
            this.rightBorder = rightBorder;
            tileWidth = width / numberOfTiles;
            Spawn(startAfterXLanes - 1);
            ScoreManager.instance.OnScored += OnScored;
        }

        private void OnScored(int score)
        {
            if (score > lastSpawnedIndex)
            {
                var c = coins.First();
                coins.Remove(c);
                c.ReturnToPool();
                Spawn(lastSpawnedIndex + everyXLanes);
            }
        }

        private void Spawn(int laneIndex)
        {
            var pooledCoin = ObjectPool.instance.GetPooledObject("Coin");
            var poolObj = pooledCoin.gameObject;
            Tile tile;
            int tileIndex;
            var tryCount = 0;
            do
            {
                tryCount++;
                if (tryCount == numberOfTiles)
                {
                    Spawn(laneIndex + 1);
                    return;
                }

                tileIndex = Range(0, numberOfTiles);
                tile = MapManager.instance.GetTile(laneIndex, tileIndex);
            } while (tile.IsOccupied);

            var z = tile.transform.position.z;

            lastSpawnedIndex = laneIndex;
            var x = leftBorder + tileIndex * tileWidth + tileWidth / 2;
            var spawnPosition = new Vector3(
                x,
                poolObj.transform.position.y,
                z
            );
            poolObj.transform.position = spawnPosition;
            poolObj.SetActive(true);
            poolObj.transform.SetParent(tile.transform);
            tile.coin = pooledCoin;
            tile.hasCoin = true;
            coins.Add(pooledCoin);

            var coin = poolObj.GetComponent<Coin>();
            coin.pooledObject = pooledCoin;
            coin.tile = tile;
            coin.OnCoinCollected += OnCoinCollected;
        }

        private void OnCoinCollected(Coin coin)
        {
            var c = coin.pooledObject;
            c.ReturnToPool();
            coins.Remove(c);
            Spawn(lastSpawnedIndex + everyXLanes);
        }
    }
}