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
    public class WoodSpawner : MonoBehaviour
    {
        private WoodSpawnerSettings settings;

        private GameObject woodPrefab;
        private int numberOfWoodsPerLane;
        private float minHeight;
        private float maxHeight;

        private RoadDirection direction;
        private Queue<GameObject> woodQueue;
        private List<PooledObject> woods;
        private float woodSpawnPositionX;
        private Vector3 objectsStartPosition;
        private Vector3 objectsEndPosition;
        private Quaternion woodRotation;

        private void Awake()
        {
            settings = Resources.Load<WoodSpawnerSettings>("WoodSpawnerSettings");
            woodPrefab = settings.woodPrefab;
            minHeight = settings.minHeight;
            maxHeight = settings.maxHeight;

            woodQueue = new Queue<GameObject>();
            woods = new List<PooledObject>();
        }


        public void StartSpawning(RoadDirection d, Vector3 startPosition, Vector3 endPosition)
        {
            direction = d;

            woodRotation =
                direction == RoadDirection.Right
                    ? Quaternion.Euler(0, 90, 0)
                    : Quaternion.Euler(0, 270, 0);

            var width = d == RoadDirection.Right
                ? endPosition.x - startPosition.x
                : startPosition.x - endPosition.x;

            objectsEndPosition = endPosition;
            objectsStartPosition = new Vector3(startPosition.x, 0.15f, startPosition.z);

            var followDistance = woodPrefab.GetComponent<BoxCollider>().size.x * 4;
            woodSpawnPositionX = startPosition.x + (d == RoadDirection.Right ? +followDistance : -followDistance);

            numberOfWoodsPerLane = Mathf.CeilToInt(width / followDistance * 1.5f);

            StartCoroutine(CreateWaterItems());
        }

        private IEnumerator CreateWaterItems()
        {
            yield return new WaitUntil(() => ObjectPool.instance.isPoolSet);

            EnqueueWoods();
            StartCoroutine(StartMovingWoods());
        }

        private void EnqueueWoods()
        {
            for (var i = 0; i < numberOfWoodsPerLane; i++)
            {
                var obj = ObjectPool.instance.GetPooledObject("Wood");
                obj.transform.rotation = woodRotation;
                obj.transform.position = objectsStartPosition;
                woodQueue.Enqueue(obj.gameObject);
                woods.Add(obj);
            }
        }

        private IEnumerator StartMovingWoods()
        {
            var woodObj = woodQueue.Dequeue();

            var localScale = woodObj.transform.localScale;
            var localScaleX = localScale.x;
            var localScaleY = localScale.y;

            var randomNumber = Range(minHeight, maxHeight);
            localScale = new Vector3(localScaleX, localScaleY, randomNumber);
            woodObj.transform.localScale = localScale;

            woodObj.gameObject.SetActive(true);
            var wood = woodObj.gameObject.GetComponent<Wood>();
            wood.StartMoving(woodSpawnPositionX, objectsEndPosition.x, direction);

            wood.OnFinishedLane += OnFinishedLane;

            yield return new WaitUntil(() => wood.pastSpacing);
            StartCoroutine(StartMovingWoods());
        }

        private void OnFinishedLane(GameObject wood)
        {
            wood.transform.position = objectsStartPosition;
            woodQueue.Enqueue(wood);
        }

        private void OnDestroy()
        {
            if (ObjectPool.instance == null) return;
            foreach (var wood in woods)
            {
                ObjectPool.instance.TakeBack(wood);
            }
        }
    }
}