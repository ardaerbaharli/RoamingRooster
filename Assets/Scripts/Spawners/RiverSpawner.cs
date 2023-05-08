using Enums;
using MapEntities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Spawners
{
    public class RiverSpawner : MonoBehaviour
    {
        private RiverType riverType;

        public void StartSpawning(RoadDirection direction, Vector3 objectsStartPosition, Vector3 objectsEndPosition,
            Lane lane)
        {
            riverType = (RiverType) Random.Range(0, 2);
            lane.RiverType = riverType;
            switch (riverType)
            {
                case RiverType.LilyPad:
                    var lilyPadSpawner = gameObject.AddComponent<LilyPadSpawner>();
                    lilyPadSpawner.CreateItems(lane);
                    break;
                case RiverType.Wood:
                    var woodSpawner = gameObject.AddComponent<WoodSpawner>();
                    woodSpawner.StartSpawning(direction, objectsStartPosition, objectsEndPosition);
                    break;
            }
        }
    }
}