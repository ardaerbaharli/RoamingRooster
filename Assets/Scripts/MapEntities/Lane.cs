using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Spawners;
using UnityEngine;
using static UnityEngine.Random;

namespace MapEntities
{
    public class Lane : MonoBehaviour
    {
        public List<GameObject> tileObjects;

        public List<Tile> tiles
        {
            get => tileObjects.Select(x => x.GetComponent<Tile>()).ToList();
            set => tiles = value;
        }

        public int numberOfTiles => tileObjects.Count;
        public RiverType RiverType;

        public TileType type;
        public Lane previousLane;
        public Lane nextLane;
        public int laneIndex;
        private RoadDirection direction;
        private Vector3 objectsStartPosition;
        private Vector3 objectsEndPosition;

        private void Awake()
        {
            tileObjects = new List<GameObject>();
        }

        public void SetItems()
        {
            direction = (RoadDirection) Range(1, 3);
            var startPivot = direction == RoadDirection.Right ? tileObjects.First() : tileObjects.Last();
            var endPivot = direction == RoadDirection.Right ? tileObjects.Last() : tileObjects.First();
            var tileWidth = startPivot.GetComponent<BoxCollider>().bounds.size.x;

            objectsStartPosition = startPivot.transform.position +
                                   new Vector3(direction == RoadDirection.Right ? -tileWidth * 2 : tileWidth * 2, 0, 0);
            objectsEndPosition = endPivot.transform.position +
                                 new Vector3(direction == RoadDirection.Right ? tileWidth * 2 : -tileWidth * 2, 0, 0);

            switch (type)
            {
                case TileType.Grass:
                    var grassSpawner = gameObject.AddComponent<GrassSpawner>();
                    grassSpawner.CreateItems(this);
                    break;
                case TileType.River:
                    var riverSpawner = gameObject.AddComponent<RiverSpawner>();
                    riverSpawner.StartSpawning(direction, objectsStartPosition, objectsEndPosition, this);
                    break;
                case TileType.Road:
                    var carSpawnerRoad = gameObject.AddComponent<CarSpawner>();
                    carSpawnerRoad.StartSpawning(direction, objectsStartPosition, objectsEndPosition);
                    break;
                case TileType.LanelessRoad:
                    var carSpawnerLanelessRoad = gameObject.AddComponent<CarSpawner>();
                    carSpawnerLanelessRoad.StartSpawning(direction, objectsStartPosition, objectsEndPosition);
                    break;
                case TileType.Pavement:
                    var lampSpawner = gameObject.AddComponent<LampSpawner>();
                    lampSpawner.CreateLamps(this);
                    break;
                case TileType.Rail:
                    var trainSpawner = gameObject.AddComponent<TrainSpawner>();
                    trainSpawner.StartSpawning(direction, objectsStartPosition, objectsEndPosition);
                    break;
            }
        }


        private void OnDestroy()
        {
            foreach (var tile in tileObjects)
            {
                Destroy(tile);
            }
        }

        public Transform GetCenterTileTransform()
        {
            return tileObjects[tileObjects.Count / 2].transform;
        }

        public Tile GetCenterTile()
        {
            return tileObjects[tileObjects.Count / 2].GetComponent<Tile>();
        }

        public void SetRiverTrigger(bool value)
        {
            foreach (var tile in tileObjects)
            {
                tile.GetComponent<Tile>().SetTrigger(value);
            }
        }
    }
}