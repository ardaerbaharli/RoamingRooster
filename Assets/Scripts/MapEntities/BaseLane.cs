using System.Linq;
using Spawners;

namespace MapEntities
{
    public class BaseLane : Lane
    {
        public void SetItems(int[] emptyTileIndexes)
        {
            var baseLaneSpawner = gameObject.AddComponent<BaseSpawner>();

            baseLaneSpawner.CreateItems(tiles.Where(x => !emptyTileIndexes.Contains(x.tileIndex)).ToList());
        }
    }
}