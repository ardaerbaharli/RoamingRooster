using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "CarSpawnerSettings", menuName = "SpawnerSettings/CarSpawnerSettings", order = 0)]
    public class CarSpawnerSettings : ScriptableObject
    {
        public GameObject biggestCar;
        public int numberOfCarPrefabs;
        public float topSpeed;
        public float bottomSpeed;
    }
}