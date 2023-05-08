using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "WoodSpawnerSettings", menuName = "SpawnerSettings/WoodSpawnerSettings", order = 0)]
    public class WoodSpawnerSettings : ScriptableObject
    {
        public GameObject woodPrefab;
        public float minHeight;
        public float maxHeight;
    }
}