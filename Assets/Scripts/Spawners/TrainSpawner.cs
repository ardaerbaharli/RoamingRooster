using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using MapEntities;
using ScriptableObjects;
using Train;
using UnityEngine;
using Utilities;
using static UnityEngine.Random;

namespace Spawners
{
    public class TrainSpawner : MonoBehaviour
    {
        private List<PooledObject> carriages;
        private RoadDirection direction;
        private TrainSpawnerSettings settings;
        private GameObject lightPolePrefab;
        private GameObject biggestCarriagePrefab;
        private int numberOfCarriagePerLane;
        private float spaceBetweenCars;
        private float trainSpeed;
        private float trainY;
        private float startTheTrainAfterSecondsMax;
        private float startTheTrainAfterSecondsMin;
        private float spawnEveryXSeconds;

        private float startTheTrainAfterSeconds;
        private Lane lane;
        private RailLightController railLightController;
        private TrainLocomotive locomotive;
        private PooledObject trainLocomotive;
        private Vector3 carSpacing;
        private Vector3 objectsStartPosition;
        private Vector3 objectsEndPosition;
        private Quaternion carRotation;

        private void Awake()
        {
            settings = Resources.Load<TrainSpawnerSettings>("TrainSpawnerSettings");
            lightPolePrefab = settings.lightPolePrefab;
            biggestCarriagePrefab = settings.biggestCarriagePrefab;
            numberOfCarriagePerLane = settings.numberOfCarriagePerLane;
            spaceBetweenCars = settings.spaceBetweenCars;
            trainSpeed = settings.trainSpeed;
            trainY = settings.trainY;
            startTheTrainAfterSecondsMax = settings.startTheTrainAfterSecondsMax;
            startTheTrainAfterSecondsMin = settings.startTheTrainAfterSecondsMin;
            spawnEveryXSeconds = settings.spawnEveryXSeconds;

            carriages = new List<PooledObject>();
            lane = GetComponent<Lane>();
        }

        private void Start()
        {
            startTheTrainAfterSeconds = Range(startTheTrainAfterSecondsMin, startTheTrainAfterSecondsMax);
        }

        public void StartSpawning(RoadDirection d, Vector3 startPosition, Vector3 endPosition)
        {
            direction = d;
            carRotation =
                direction == RoadDirection.Right
                    ? Quaternion.Euler(0, 90, 0)
                    : Quaternion.Euler(0, 270, 0);

            var biggestTrainSize = biggestCarriagePrefab.GetComponent<BoxCollider>().size.z;
            carSpacing = (biggestTrainSize + spaceBetweenCars) *
                         (direction == RoadDirection.Right ? Vector3.left : Vector3.right);

            objectsEndPosition = new Vector3(endPosition.x, trainY, endPosition.z);
            objectsStartPosition = new Vector3(startPosition.x, trainY, startPosition.z);

            StartCoroutine(CreateItems());
        }

        private IEnumerator CreateItems()
        {
            yield return new WaitUntil(() => ObjectPool.instance.isPoolSet);

            GetTrain();
            AssembleTrain();
            SetLightPole();
            locomotive.OnFinishedLane += OnTrainFinishedLane;

            yield return new WaitForSeconds(startTheTrainAfterSeconds);
            StartCoroutine(StartMovingTrain());
        }

        private void SetLightPole()
        {
            var tile = lane.GetCenterTileTransform();
            var tilePosition = tile.position;
            var rotation = lightPolePrefab.transform.rotation;
            var polePrefabPosition = lightPolePrefab.transform.position;
            var pos = new Vector3(tilePosition.x + polePrefabPosition.x, lightPolePrefab.transform.position.y,
                tilePosition.z - polePrefabPosition.z);
            var lightPole = Instantiate(lightPolePrefab, pos, rotation, tile);
            railLightController = lightPole.GetComponent<RailLightController>();
        }

        private IEnumerator StartMovingTrain()
        {
            while (gameObject.activeSelf)
            {
                railLightController.StartBlinking();
                yield return new WaitForSeconds(2f);
                trainLocomotive.transform.position = objectsStartPosition;
                trainLocomotive.gameObject.SetActive(true);
                locomotive.StartMoving(objectsEndPosition.x, direction, trainSpeed, carriages.Last().transform);
                yield return new WaitForSeconds(spawnEveryXSeconds);
            }
        }


        private void OnTrainFinishedLane()
        {
            railLightController.Stop();
        }

        private void AssembleTrain()
        {
            var lastCarriagePosition = trainLocomotive.transform.position;
            var i = 1;
            foreach (var carriageTransform in carriages.Select(carriages => carriages.transform))
            {
                carriageTransform.SetParent(trainLocomotive.transform);
                var carriagePos = lastCarriagePosition + i * carSpacing;
                carriageTransform.position = carriagePos;
                lastCarriagePosition = carriagePos;
                carriageTransform.gameObject.SetActive(true);
            }
        }

        private void GetTrain()
        {
            trainLocomotive = ObjectPool.instance.GetPooledObject("Locomotive");
            trainLocomotive.transform.rotation = carRotation;
            trainLocomotive.transform.position = objectsStartPosition;
            locomotive = trainLocomotive.gameObject.GetComponent<TrainLocomotive>();

            for (var i = 0; i < numberOfCarriagePerLane; i++)
            {
                var obj = ObjectPool.instance.GetPooledObject("Carriage");
                obj.transform.rotation = carRotation;
                carriages.Add(obj);
            }
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            if (ObjectPool.instance == null) return;

            foreach (var car in carriages)
            {
                ObjectPool.instance.TakeBack(car);
            }
        }
    }
}