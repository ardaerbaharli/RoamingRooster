using System;
using System.Linq;
using MapEntities;
using ScriptableObjects;
using UnityEngine;

namespace Spawners
{
    public class LampSpawner : MonoBehaviour
    {
         private LampSpawnerSettings settings;
        private GameObject streetLampPrefab;

        private void Awake()
        {
            settings = Resources.Load<LampSpawnerSettings>("LampSpawnerSettings");
            streetLampPrefab = settings.streetLampPrefab;

        }


        public void CreateLamps(Lane lane)
        {
            var prefPos = streetLampPrefab.transform.position;

            var firstTileObj = lane.tileObjects.First();
            var lastTileObj = lane.tileObjects.Last();

            var firstTilePosition = firstTileObj.transform.position;
            var firstLampPosition = new Vector3(firstTilePosition.x, prefPos.y, firstTilePosition.z);

            var lastTilePosition = lastTileObj.transform.position;
            var lastLampPosition = new Vector3(lastTilePosition.x, prefPos.y, lastTilePosition.z);

            var firstLamp = Instantiate(streetLampPrefab, firstTileObj.transform);
            firstLamp.transform.position = firstLampPosition;
            firstLamp.transform.rotation = Quaternion.Euler(-90, 0, -180);
            var lastLamp = Instantiate(streetLampPrefab, lastTileObj.transform);
            lastLamp.transform.position = lastLampPosition;
            lastLamp.transform.rotation = Quaternion.Euler(-90, 0, 0);

            var firstTile = firstTileObj.GetComponent<Tile>();
            firstTile.hasLamp = true;
            firstTile.hasObstacle = true;

            var lastTile = lastTileObj.GetComponent<Tile>();
            lastTile.hasLamp = true;
            lastTile.hasObstacle = true;
        }
    }
}