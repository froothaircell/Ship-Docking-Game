using UnityEngine;

namespace GameResources.Ship
{
    public class ShipSpawner : MonoBehaviour
    {
        private void OnDestroy()
        {
            AppHandler.ShipSpawnHandler.RemoveFromSpawnersList(this.gameObject);
        }

        public void Spawn(ShipTypes type, ShipColors shipColor)
        {
            AppHandler.ShipPoolHandler.GetFromPool(type, shipColor, transform.position, transform.rotation);
        }
    }
}