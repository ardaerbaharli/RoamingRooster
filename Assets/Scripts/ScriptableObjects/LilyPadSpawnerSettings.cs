using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "LilyPadSpawnerSettings", menuName = "SpawnerSettings/LilyPadSpawnerSettings", order = 0)]
    public class LilyPadSpawnerSettings : ScriptableObject
    {
        public List<GameObject> lilyPadPrefabs;
        [Header("Min-max is for every 10 tiles")]
        public int numberOfMaxItems;
        public int numberOfMinItems;
        
    }
}