using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using MapEntities;
using Spawners;
using UnityEngine;
using Utilities;
using static UnityEngine.Random;

namespace Controllers
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private Player.Player player;

        [SerializeField] private Transform mapParent;
        [SerializeField] private List<GameObject> terrains;
        [SerializeField] private int initialNumberOfLanes;
        [SerializeField] private int numberOfTilesPerLane;
        [SerializeField] private int numberOfBaseLanes;
        [SerializeField] private TileType baseType;

        [SerializeField] private float tileWidth;

        private int playerStartTileIndex;

        [SerializeField, Tooltip("Position of the first tile generated.")]
        private Vector3 generationStartPoint;

        [NonSerialized] public List<Lane> lanes;

        private float leftBorderX, rightBorderX;
        private Vector3 lastSpawnedPos;
        private int createdLanesIndex;
        private Lane lastLaneCreated;
        private Vector3 tileDistance;
        private List<GameObject> safeLanes;
        private CoinSpawner coinSpawner;
        public static MapManager instance;
        private bool lanesReady;
        private int numberOfDeletedLanes;

        private void Awake()
        {
            instance = this;
            lanes = new List<Lane>();
            coinSpawner = GetComponent<CoinSpawner>();
            playerStartTileIndex = numberOfTilesPerLane / 2;
        }

        private void OnValidate()
        {
            leftBorderX = generationStartPoint.x - tileWidth / 2;
            rightBorderX = leftBorderX + numberOfTilesPerLane * tileWidth;
        }

        private IEnumerator Start()
        {
            createdLanesIndex = -numberOfBaseLanes;

            tileDistance = new Vector3(tileWidth, 0, 0);

            leftBorderX = generationStartPoint.x - tileWidth / 2;
            rightBorderX = leftBorderX + numberOfTilesPerLane * tileWidth;

            ObjectPool.instance.Init(numberOfTilesPerLane, initialNumberOfLanes);
            ObjectPool.instance.StartPool();

            safeLanes = terrains.Where(x =>
                x.GetComponent<Tile>().type != TileType.Road &&
                x.GetComponent<Tile>().type != TileType.LanelessRoad).ToList();

            GenerateBase();

            var playerStartTile = lanes[numberOfBaseLanes].tiles[playerStartTileIndex];
            var playerStartPos = playerStartTile.transform.position;
            playerStartPos = new Vector3(playerStartPos.x, player.transform.position.y, playerStartPos.z);
            player.SetPosition(playerStartPos, playerStartTile);

            GenerateTerrain(initialNumberOfLanes);

            yield return new WaitUntil(() => lanesReady);

            coinSpawner.StartSpawning(numberOfTilesPerLane, leftBorderX, rightBorderX);
        }

        private void GenerateBase()
        {
            for (var j = -numberOfBaseLanes; j < 0; j++)
            {
                lastSpawnedPos = GetSpawnPoint(createdLanesIndex);

                var laneObj = new GameObject();
                laneObj.transform.position = lastSpawnedPos;
                laneObj.transform.SetParent(mapParent);
                laneObj.name = createdLanesIndex.ToString();

                var lane = laneObj.AddComponent<BaseLane>();
                lane.type = baseType;
                if (lastLaneCreated != null)
                {
                    lastLaneCreated.nextLane = lane;
                    lane.previousLane = lastLaneCreated;
                }

                lastLaneCreated = lane;

                var tilePrefab = terrains.FirstOrDefault(x => x.GetComponent<Tile>().type == baseType);

                for (var i = 0; i < numberOfTilesPerLane; i++)
                {
                    var tileObject = Instantiate(tilePrefab, laneObj.transform, true);
                    lane.tileObjects.Add(tileObject);

                    tileObject.transform.position += lastSpawnedPos + (tileDistance * i);
                    var tile = tileObject.GetComponent<Tile>();
                    tileObject.name = $"{createdLanesIndex}_{i}";
                    tile.lane = lane;
                    tile.tileIndex = i;
                    tile.type = baseType;
                }

                lane.SetItems(new int[] { });
                lane.laneIndex = createdLanesIndex;
                lanes.Add(lane);
                createdLanesIndex++;
            }

            var emptyTileIndexes = new[] {playerStartTileIndex, playerStartTileIndex - 1, playerStartTileIndex + 1};
            for (var j = 0; j < numberOfBaseLanes; j++)
            {
                lastSpawnedPos = GetSpawnPoint(createdLanesIndex);

                var laneObj = new GameObject();
                laneObj.transform.position = lastSpawnedPos;
                laneObj.transform.SetParent(mapParent);
                laneObj.name = createdLanesIndex.ToString();

                var lane = laneObj.AddComponent<BaseLane>();
                lane.type = baseType;
                if (lastLaneCreated != null)
                {
                    lastLaneCreated.nextLane = lane;
                    lane.previousLane = lastLaneCreated;
                }

                lastLaneCreated = lane;

                var tilePrefab = terrains.FirstOrDefault(x => x.GetComponent<Tile>().type == baseType);

                for (var i = 0; i < numberOfTilesPerLane; i++)
                {
                    var tileObject = Instantiate(tilePrefab, laneObj.transform, true);
                    lane.tileObjects.Add(tileObject);

                    tileObject.transform.position += lastSpawnedPos + (tileDistance * i);
                    var tile = tileObject.GetComponent<Tile>();
                    tileObject.name = $"{createdLanesIndex}_{i}";
                    tile.lane = lane;
                    tile.tileIndex = i;
                    tile.type = baseType;
                }

                lane.SetItems(emptyTileIndexes);
                lane.laneIndex = createdLanesIndex;

                lanes.Add(lane);
                createdLanesIndex++;

                var direction = Range(-1, 2);
                emptyTileIndexes = new[]
                    {emptyTileIndexes[0] + direction, emptyTileIndexes[1] + direction, emptyTileIndexes[2] + direction};
            }
        }

        public void GenerateTerrain(int numberOfLanes)
        {
            for (var j = 0; j < numberOfLanes; j++)
            {
                lastSpawnedPos = GetSpawnPoint(createdLanesIndex);

                var tilePrefab = SelectLaneType();

                var laneObj = new GameObject();
                laneObj.transform.SetParent(mapParent);
                var lane = laneObj.AddComponent<Lane>();
                if (lastLaneCreated != null)
                {
                    lastLaneCreated.nextLane = lane;
                    lane.previousLane = lastLaneCreated;
                }

                lastLaneCreated = lane;
                laneObj.name = createdLanesIndex.ToString();
                laneObj.transform.position = lastSpawnedPos; // TODO: change to level position

                var selectedTerrain = tilePrefab.GetComponent<Tile>();
                var terrainType = selectedTerrain.type;
                lane.type = terrainType;

                for (var i = 0; i < numberOfTilesPerLane; i++)
                {
                    var tileObject = Instantiate(tilePrefab, laneObj.transform, true);
                    lane.tileObjects.Add(tileObject);
                    tileObject.transform.position += lastSpawnedPos + (tileDistance * i);
                    var tile = tileObject.GetComponent<Tile>();
                    tileObject.name = $"{createdLanesIndex}_{i}";
                    tile.lane = lane;
                    tile.tileIndex = i;
                    tile.type = terrainType;
                }

                lane.SetItems();
                lane.laneIndex = createdLanesIndex;
                lanes.Add(lane);
                createdLanesIndex++;
            }

            lanesReady = true;
        }

        private GameObject SelectLaneType()
        {
            var tileType = GetRandomTileType();
            var selectedTileObj = terrains.FirstOrDefault(x => x.GetComponent<Tile>().type == tileType);

            if (lastLaneCreated.type is TileType.Road && tileType is not TileType.Road)
            {
               return terrains.First(x => x.GetComponent<Tile>().type == TileType.LanelessRoad);
            }

            if (lastLaneCreated.type is TileType.LanelessRoad &&
                tileType is TileType.Road or TileType.LanelessRoad)
            {
                return GetRandomSafeTerrain();
            }


            return selectedTileObj;
        }

        private TileType GetRandomTileType()
        {
            return (TileType) Range(1, Enum.GetValues(typeof(TileType)).Length);
        }

        private GameObject GetRandomSafeTerrain()
        {
            var randomTerrainType = Range(0, safeLanes.Count);
            return safeLanes[randomTerrainType];
        }

        private Vector3 GetSpawnPoint(int i)
        {
            return new Vector3(lastSpawnedPos.x, generationStartPoint.y, tileWidth + tileWidth * i);
        }

        public enum Filter
        {
            First,
            Last
        }

        public void DestroyLanes(int numberOfLanes, Filter filter)
        {
            if (numberOfLanes + numberOfBaseLanes > ScoreManager.instance.Score) return;
            var lanesToDestroy = filter == Filter.First
                ? lanes.Take(numberOfLanes).ToList()
                : lanes.Skip(lanes.Count - numberOfLanes).ToList();

            foreach (var lane in lanesToDestroy)
            {
                lanes.Remove(lane);
                Destroy(lane.gameObject);
            }

            numberOfDeletedLanes += numberOfLanes;
        }

        public Tile GetTile(int laneIndex, int tileIndex)
        {
            return lanes[laneIndex - numberOfDeletedLanes].tileObjects[tileIndex].GetComponent<Tile>();
        }

        public Tile GetTile(int laneIndex, float estimateTilePositionX)
        {
            var tilePosX = (int) estimateTilePositionX;
            if (estimateTilePositionX % 2 != 0)
            {
                // round tilePositionX to nearest even number
                tilePosX = (int) Mathf.Round(estimateTilePositionX / 2) * 2;
            }

            var tileIndex = (int) (tilePosX - leftBorderX) / 2;
            return GetTile(laneIndex, tileIndex);
        }

        public void SetRiverTrigger(bool value)
        {
            lanes.Where(x=>x.type == TileType.River).ToList().ForEach(x=>x.SetRiverTrigger(value));
        }
    }
}