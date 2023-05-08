using System;
using Controllers;
using UnityEngine;
using Utilities;

namespace MapEntities
{
    public class Coin : MonoBehaviour
    {
        public Action<Coin> OnCoinCollected;
        public Tile tile;
    
        public PooledObject pooledObject;
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;

            SoundManager.instance.PlayCoinCollectSound();
            
            OnCoinCollected?.Invoke(this);
            CoinManager.instance.Coins += 1;
            tile.coin = null;
            tile.hasCoin = false;
        }
    
  
    }
}