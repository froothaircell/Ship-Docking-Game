using System;
using System.Collections.Generic;
using CoreResources.Handlers.EventHandler;
using CoreResources.Pool;
using CoreResources.Utils.Disposables;
using GameResources.Events;
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

        private void Awake()
        {
            GetComponent<NavMeshAgent>().speed = _shipData.ShipSpeed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Flag"))
            {
                REvent_BoatDocked.Dispatch(transform.position);
                AppHandler.ShipPoolHandler.Despawn(gameObject);
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                REvent_BoatDestroyed.Dispatch(transform.position);
                AppHandler.ShipPoolHandler.Despawn(gameObject);
            }
        }
    }
}