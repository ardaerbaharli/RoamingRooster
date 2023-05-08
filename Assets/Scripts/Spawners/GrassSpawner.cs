using System;
using System.Collections.Generic;
using MapEntities;
using ScriptableObjects;
using UnityEngine;
using static UnityEngine.Random;

namespace Spawners
{
    public class GrassSpawner : MonoBehaviour
    {
        private GrassSpawnerSettings settings;
        private List<GameObject> grassItemPrefabs;

        private void Awake()
        {
            settings = Resources.Load<GrassSpawnerSettings>("GrassSpawnerSettings");
            grassItemPrefabs = settings.grassItemPrefabs;
        }


        public void CreateItems(Lane lane)
        {
            var multiplier = lane.numberOfTiles / 10f;
            var numberOfItems = (int) Range(settings.numberOfMinItems * multiplier,
                settings.numberOfMaxItems * multiplier);

            for (var i = 0; i < numberOfItems; i++)
            {
                var tries = 0;
                var randomGrassItem = grassItemPrefabs[Range(0, grassItemPrefabs.Count - 1)];
                var tileIndex = Range(0, lane.tiles.Count - 1);
                var randomTile = lane.tileObjects[tileIndex];
                var tile = lane.tiles[tileIndex];

                while (tile.IsOccupied || lane.previousLane.tiles[tileIndex].hasObstacle
                                       || lane.previousLane.tiles[Mathf.Clamp(tileIndex - 1, 0, lane.numberOfTiles - 1)]
                                           .hasObstacle
                                       || lane.previousLane.tiles[Mathf.Clamp(tileIndex + 1, 0, lane.numberOfTiles - 1)]
                                           .hasObstacle)
                {
                    tileIndex = Range(0, lane.tiles.Count - 1);
                    randomTile = lane.tileObjects[tileIndex];
                    tile = lane.tiles[tileIndex];
                    tries++;
                    if (tries != lane.numberOfTiles) continue;
                    i++;
                    break;
                }

                if (tries == 9) continue;

                tile.hasGrassItem = true;
                var tilePos = randomTile.transform.position;
                var itemPos = new Vector3(tilePos.x, randomGrassItem.transform.position.y, tilePos.z);
                var rot = randomGrassItem.transform.rotation;
                var item = Instantiate(randomGrassItem, itemPos, rot, randomTile.transform);
                tile.hasObstacle = item.GetComponent<GrassItem>().isObstacle;
            }
        }
    }
}