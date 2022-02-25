using System;
using UnityEngine;

namespace GameResources.Ship
{
    public class ShipSpawner : MonoBehaviour
    {
        private void Start()
        {
            AppHandler.ShipSpawnHandler.AddToSpawnerList(this.gameObject);
        }

        public void Spawn(ShipTypes type)
        {
            AppHandler.ShipPoolHandler.Spawn(type, transform.position, transform.rotation);
        }
    }
}