using System;
using Enums;
using UnityEngine;
using Utilities;

namespace MapEntities
{
    public class Tile : MonoBehaviour
    {
        public TileType type;
        public bool hasGrassItem;
        public Lane lane;
        public int tileIndex;
        public bool hasObstacle;
        public bool hasCoin;
        public PooledObject coin;
        public bool hasLamp;
        public bool hasLilyPad;

        public bool IsOccupied => hasGrassItem || hasObstacle || hasCoin || hasLamp || hasLilyPad;

        private void OnDestroy()
        {
            if (!hasCoin) return;
            if (ObjectPool.instance == null) return;
            ObjectPool.instance.TakeBack(coin);
        }

        public void SetTrigger(bool value)
        {
            GetComponent<Collider>().isTrigger = value;
        }
    }
}