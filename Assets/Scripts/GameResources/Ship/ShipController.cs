using System;
using GameResources.Events;
using GameResources.Pathing;
using UnityEngine;
using UnityEngine.AI;

namespace GameResources.Ship
{
    [System.Serializable]
    public class ShipData
    {
        public ShipTypes ShipType;
        public float ShipSpeed;
    }

    public class ShipController : MonoBehaviour
    {
        public ShipData _shipData;
        private RPathingManager _pathingManager;

        private void Awake()
        {
            _pathingManager = GetComponent<RPathingManager>();
        }

        private void OnEnable()
        {
            _pathingManager.InitPathing(_shipData.ShipSpeed);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Flag"))
            {
                REvent_BoatDocked.Dispatch(transform.position);
                AppHandler.ShipPoolHandler.AddToPool(gameObject);
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle") ||
                     other.gameObject.layer == LayerMask.NameToLayer("Boat"))
            {
                REvent_BoatDestroyed.Dispatch(transform.position);
                AppHandler.ShipPoolHandler.AddToPool(gameObject);
            }
        }
    }
}