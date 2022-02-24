using System;
using UnityEngine;

namespace GameResources.Ship
{
    public class ShipPoolTest : MonoBehaviour
    {
        private void Start()
        {
            AppHandler.ShipPoolHandler.ResetPool();
        }
    }
}