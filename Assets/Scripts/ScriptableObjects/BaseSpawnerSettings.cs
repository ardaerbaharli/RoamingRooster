using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "BaseSpawnerSettings", menuName = "SpawnerSettings/BaseSpawnerSettings", order = 0)]
    public class BaseSpawnerSettings : ScriptableObject
    {
        public List<GameObject> fillItemPrefabs;
    }
}