using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using MapEntities;
using ScriptableObjects;
using UnityEngine;
using Utilities;
using static UnityEngine.Random;

namespace Spawners
{
    public class CarSpawner : MonoBehaviour
    {
        private CarSpawnerSettings settings;

        private Queue<GameObject> carQueue;
        private List<PooledObject> cars;
        private RoadDirection direction;
        private GameObject biggestCar;
        private int numberOfCarPrefabs;
        private float topSpeed;
        private float bottomSpeed;
        private int numberOfCarsPerLane;

        private float carSpawnPositionX;
        private Vector3 objectsStartPosition;
        private Vector3 objectsEndPosition;
        private Quaternion carRotation;

        private void Awake()
        {
            settings = Resources.Load<CarSpawnerSettings>("CarSpawnerSettings");
            biggestCar = settings.biggestCar;
            numberOfCarPrefabs = settings.numberOfCarPrefabs;
            topSpeed = settings.topSpeed;
            bottomSpeed = settings.bottomSpeed;
            // numberOfCarsPerLane = settings.numberOfCarsPerLane;

            carQueue = new Queue<GameObject>();
            cars = new List<PooledObject>();
        }

        public void StartSpawning(RoadDirection d, Vector3 startPosition, Vector3 endPosition)
        {
            direction = d;
            carRotation =
                direction == RoadDirection.Right
                    ? Quaternion.Euler(0, 90, 0)
                    : Quaternion.Euler(0, 270, 0);

            var width = direction == RoadDirection.Right
                ? endPosition.x - startPosition.x
                : startPosition.x - endPosition.x;

            objectsEndPosition = endPosition;
            objectsStartPosition = startPosition;

            var followDistance = biggestCar.GetComponent<BoxCollider>().size.x * 4;
            carSpawnPositionX = startPosition.x + (d == RoadDirection.Right ? +followDistance : -followDistance);

            numberOfCarsPerLane = Mathf.CeilToInt(width / followDistance * 1.5f);
            StartCoroutine(CreateRoadItems());
        }

        private IEnumerator CreateRoadItems()
        {
            yield return new WaitUntil(() => ObjectPool.instance.isPoolSet);

            EnqueueCars();
            StartCoroutine(StartMovingCar());
        }

        private void EnqueueCars()
        {
            for (var i = 0; i < numberOfCarsPerLane; i++)
            {
                var roadItem = Range(0, numberOfCarPrefabs);
                var obj = ObjectPool.instance.GetPooledObject("Car" + roadItem);
                obj.transform.rotation = carRotation;
                obj.transform.position = objectsStartPosition;
                carQueue.Enqueue(obj.gameObject);
                cars.Add(obj);
            }
        }


        private IEnumerator StartMovingCar()
        {
            var carObj = carQueue.Dequeue();
            carObj.gameObject.SetActive(true);
            var car = carObj.gameObject.GetComponent<Car>();
            var speed = Range(bottomSpeed, topSpeed);
            car.StartMoving(carSpawnPositionX, objectsEndPosition.x, direction, speed);

            car.OnFinishedLane += CarFinishedLane;
            yield return new WaitUntil(() => car.pastSpacing);
            StartCoroutine(StartMovingCar());
        }

        private void CarFinishedLane(GameObject car)
        {
            car.transform.position = objectsStartPosition;
            carQueue.Enqueue(car);
        }


        private void OnDestroy()
        {
            StopAllCoroutines();
            if (ObjectPool.instance == null) return;

            foreach (var car in cars)
            {
                ObjectPool.instance.TakeBack(car);
            }

        }
    }
}