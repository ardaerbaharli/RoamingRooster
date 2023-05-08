using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "TrainSpawnerSettings", menuName = "SpawnerSettings/TrainSpawnerSettings", order = 0)]
    public class TrainSpawnerSettings : ScriptableObject
    {
         public GameObject lightPolePrefab;
         public GameObject biggestCarriagePrefab;
         public int numberOfCarriagePerLane;
         public float spaceBetweenCars;
         public float trainSpeed;
         public float trainY;
         public float startTheTrainAfterSecondsMax;
         public float startTheTrainAfterSecondsMin;
         public float spawnEveryXSeconds;
        
        
    }
}