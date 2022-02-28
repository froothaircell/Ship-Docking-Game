using System;
using UnityEngine;

namespace GameResources.Ship
{
    public class ShipSpawner : MonoBehaviour
    {
        public void AddSelfToManager()
        {
            // AppHandler.ShipSpawnHandler.AddToSpawnerList(this.gameObject);
        }

        private void OnDestroy()
        {
            AppHandler.ShipSpawnHandler.RemoveFromSpawnersList(this.gameObject);
        }

        public void Spawn(ShipTypes type)
        {
            AppHandler.ShipPoolHandler.Spawn(type, transform.position, transform.rotation);
        }
    }
}