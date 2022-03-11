using GameResources.Ship;
using UnityEngine;

namespace GameResources.Pathing
{
    public class RPathDrive : MonoBehaviour, IShipComponent
    {
        private float _movementSpeed;
        
        public void OnInit()
        {
            _movementSpeed = GetComponent<AltShipController>().shipData.ShipSpeed;
        }

        public void OnReset()
        {
            
        }

        public void OnUpdate()
        {
            transform.position += transform.forward * Time.deltaTime * _movementSpeed;
        }
    }
}