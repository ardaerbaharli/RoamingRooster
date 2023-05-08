using System;
using System.Collections.Generic;
using System.Linq;
using MapEntities;
using ScriptableObjects;
using UnityEngine;
using static UnityEngine.Random;

namespace Spawners
{
    public class BaseSpawner : MonoBehaviour
    {
        public List<GameObject> fillItemPrefabs;
        private BaseSpawnerSettings settings;

        private void Awake()
        {
            settings = Resources.Load<BaseSpawnerSettings>("BaseSpawnerSettings");
            fillItemPrefabs = settings.fillItemPrefabs;
        }

        public void CreateItems(List<Tile> tilesToFill)
        {
            foreach (var tile in tilesToFill)
            {
                var prefab = GetRandomPrefab();
                Instantiate(prefab, tile.transform.position, prefab.transform.rotation, tile.transform);
                tile.hasObstacle = true;
            }
        }

        private GameObject GetRandomPrefab()
        {
            return fillItemPrefabs[Range(0, fillItemPrefabs.Count)];
        }
    }
}