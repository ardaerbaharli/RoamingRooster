using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "LampSpawnerSettings", menuName = "SpawnerSettings/LampSpawnerSettings", order = 0)]
    public class LampSpawnerSettings : ScriptableObject
    {
        public GameObject streetLampPrefab;
    }
}