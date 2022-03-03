using UnityEngine;

namespace GameResources.Ship
{
    public class ShipSpawner : MonoBehaviour
    {
        private void OnDestroy()
        {
            AppHandler.ShipSpawnHandler.RemoveFromSpawnersList(this.gameObject);
        }

        public void Spawn(ShipTypes type)
        {
            AppHandler.ShipPoolHandler.GetFromPool(type, transform.position, transform.rotation);
        }
    }
}