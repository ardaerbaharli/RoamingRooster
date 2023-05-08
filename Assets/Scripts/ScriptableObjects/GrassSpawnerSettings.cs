using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "GrassSpawnerSettings", menuName = "SpawnerSettings/GrassSpawnerSettings", order = 0)]
    public class GrassSpawnerSettings : ScriptableObject
    {
       public List<GameObject> grassItemPrefabs;
       [Header("Min-max is for every 10 tiles")]
       public int numberOfMaxItems;
       public int numberOfMinItems;
    }
}