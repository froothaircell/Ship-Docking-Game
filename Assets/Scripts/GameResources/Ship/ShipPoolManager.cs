using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreResources.Handlers.EventHandler;
using CoreResources.Pool;
using CoreResources.Utils;
using CoreResources.Utils.Disposables;
using CoreResources.Utils.Singletons;
using GameResources.Events;
using GameResources.Pathing;
using UnityEngine;

namespace GameResources.Ship
{
    public enum ShipTypes
    {
        ScoutBoat = 0,
        FisherBoat = 1,
        SpeedBoat = 2,
        JetSki = 3
    }
    
    public class ShipPoolManager : GenericSingleton<ShipPoolManager>
    {
        // We'll use this to instantiate new objects of the specific type
        // Load the ships into this list
        private Dictionary<ShipTypes, GameObject> _shipTypes;
        // We'll keep this dynamic for each level
        private Dictionary<ShipTypes, List<GameObject>> _shipPool;
        private PooledList<GameObject> _spawnedItems;
        private PooledList<IDisposable> _disposables;
        private const int poolCap = 10;

        private void OnDestroy()
        {
            if (_disposables != null)
            {
                _disposables.ClearDisposables();
                _disposables.ReturnToPool();
            }
            _spawnedItems.ReturnToPool();
        }

        protected override void InitSingleton()
        {
            base.InitSingleton();
            
            if (_disposables == null)
            {
                _disposables = AppHandler.AppPool.Get<PooledList<IDisposable>>();
            }
            _shipPool = new Dictionary<ShipTypes, List<GameObject>>();
            _spawnedItems = AppHandler.AppPool.Get<PooledList<GameObject>>();

            AppHandler.EventHandler.Subscribe<REvent_LevelStart>(OnStartLevel, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPauseToMainMenu>(OnEndLevel, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPlayToWin>(OnEndLevel, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPlayToLoss>(OnEndLevel, _disposables);
            
            LoadShipPool();
        }

        private async void OnStartLevel(REvent evt)
        {
            ResetPool();
            await Task.Delay(500);
            REvent_BoatsLoaded.Dispatch();
        }

        private async void OnEndLevel(REvent evt)
        {
            ResetPool();
            await Task.Delay(500);
        }

        // Make sure to name the prefabs after the variable names listed here
        // This function is a bit more manual than I'd like it to be but it'll
        // do for now
        private void LoadShipPool()
        {
            if (_shipTypes == null)
            {
                _shipTypes = new Dictionary<ShipTypes, GameObject>();
            }
            
            foreach (var shipType in EnumUtil.GetValues<ShipTypes>())
            {
                if(!_shipTypes.ContainsKey(shipType))
                    _shipTypes.Add(shipType, null);
                //Debug.Log(shipType.ToString());
                _shipTypes[shipType] = _shipTypes[shipType] == null
                    ? AppHandler.AssetHandler.LoadAsset<GameObject>(shipType.ToString())
                    : _shipTypes[shipType];
            }

            InstantiateShips(_shipTypes);
        }

        private void InstantiateShips(Dictionary<ShipTypes, GameObject> shipTypes)
        {
            foreach (var shipType in EnumUtil.GetValues<ShipTypes>())
            {
                List<GameObject> ships = new List<GameObject>();
                for (int j = 0; j < poolCap; j++)
                {
                    GameObject newShip = Instantiate(shipTypes[shipType], transform.position, transform.rotation, transform);
                    newShip.SetActive(false);
                    ships.Add(newShip);
                }

                _shipPool.Add(shipType, ships);
            }
        }

        private void ResetPool()
        {
            if (_spawnedItems.Count > 0)
            {
                foreach (var spawnedItem in _spawnedItems)
                {
                    AddToPool(spawnedItem);
                }
            }
            // ClearPool();
            // InitSingleton();
        }

        private void ClearPool()
        {
            foreach (var shipType in EnumUtil.GetValues<ShipTypes>())
            {
                for (int i = 0; i < _shipPool[shipType].Count; i++)
                {
                    Destroy(_shipPool[shipType][i]);
                }
                _shipPool[shipType].Clear();
            }
            _shipPool.Clear();

            for (int i = 0; i < _spawnedItems.Count; i++)
            {
                Destroy(_spawnedItems[i]);
            }
            _spawnedItems.Clear();
        }

        public GameObject GetFromPool(ShipTypes type, Vector3 position, Quaternion rotation)
        {
            GameObject temp = null;
            if (_shipPool[type].Count > 0)
            {
                temp = _shipPool[type][0];
                _shipPool[type].RemoveAt(0);
            }
            else
            {
                temp = AddNewItemInPool(type);
                _shipPool[type].Remove(temp);
            }
            _spawnedItems.Add(temp);
            temp.transform.position = position;
            temp.transform.rotation = rotation;
            temp.SetActive(true);
            temp.GetComponent<RPathMover>().enabled = true;
            return temp;
        }

        private GameObject AddNewItemInPool(ShipTypes shipType)
        {
            GameObject newShip = Instantiate(_shipTypes[shipType], transform.position, transform.rotation, transform);
            newShip.SetActive(false);
            _shipPool[shipType].Add(newShip);
            return newShip;
        }

        public void AddToPool(GameObject itemToDespawn)
        {
            if (_spawnedItems.Contains(itemToDespawn))
            {
                itemToDespawn.SetActive(false);
                itemToDespawn.transform.position = transform.position;
                itemToDespawn.transform.rotation = transform.rotation;
                _spawnedItems.Remove(itemToDespawn);
                _shipPool[itemToDespawn.GetComponent<ShipController>()._shipData.ShipType].Add(itemToDespawn);
            }
            else
            {
                throw new ArgumentOutOfRangeException($"ShipPoolManager | {itemToDespawn.name} does not exist in the list of spawned items");
            }
        }
    }
}