using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using MapEntities;
using ScriptableObjects;
using UnityEngine;
using static UnityEngine.Random;

namespace Spawners
{
    public class LilyPadSpawner : MonoBehaviour
    {
        private LilyPadSpawnerSettings settings;
        private List<GameObject> lilyPadPrefabs;

        private void Awake()
        {
            settings = Resources.Load<LilyPadSpawnerSettings>("LilyPadSpawnerSettings");
            lilyPadPrefabs = settings.lilyPadPrefabs;
        }


        public void CreateItems(Lane lane)
        {
            var multiplier = lane.numberOfTiles / 10f;
            var numberOfItems = (int) Range(settings.numberOfMinItems * multiplier,
                settings.numberOfMaxItems * multiplier);

            var i = 0;
            if (lane.previousLane.type == TileType.River
                && lane.previousLane.RiverType == RiverType.LilyPad
                && lane.type == TileType.River &&
                lane.RiverType == RiverType.LilyPad)
            {
                var lilyPadTiles = lane.previousLane.tiles.Where(x => x.hasLilyPad).ToList();
                foreach (var t in lilyPadTiles)
                {
                    var randomGrassItem = lilyPadPrefabs[Range(0, lilyPadPrefabs.Count - 1)];
                    var tile = lane.tiles[t.tileIndex];
                    var tileObject = lane.tileObjects[t.tileIndex];
                    tile.hasLilyPad = true;
                    var tilePos = tileObject.transform.position;
                    var itemPos = new Vector3(tilePos.x, randomGrassItem.transform.position.y, tilePos.z);
                    var rot = randomGrassItem.transform.rotation;
                    var item = Instantiate(randomGrassItem, itemPos, rot, tileObject.transform);
                    tile.hasObstacle = item.GetComponent<RiverItem>().isObstacle;
                    i++;
                }
            }

            for (; i < numberOfItems; i++)
            {
                var randomGrassItem = lilyPadPrefabs[Range(0, lilyPadPrefabs.Count - 1)];
                var tileObject = lane.tileObjects[Range(0, lane.tileObjects.Count - 1)];
                var tile = tileObject.GetComponent<Tile>();

                while (tile.IsOccupied)
                {
                    tileObject = lane.tileObjects[Range(0, lane.tileObjects.Count - 1)];
                    tile = tileObject.GetComponent<Tile>();
                }

                tile.hasLilyPad = true;
                var tilePos = tileObject.transform.position;
                var itemPos = new Vector3(tilePos.x, randomGrassItem.transform.position.y, tilePos.z);
                var rot = randomGrassItem.transform.rotation;
                var item = Instantiate(randomGrassItem, itemPos, rot, tileObject.transform);
                tile.hasObstacle = item.GetComponent<RiverItem>().isObstacle;
            }
        }
    }
}