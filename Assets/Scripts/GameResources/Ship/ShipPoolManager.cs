using System;
using System.Collections.Generic;
using CoreResources.Handlers.EventHandler;
using CoreResources.Pool;
using CoreResources.Utils.Disposables;
using CoreResources.Utils.Singletons;
using GameResources.Events;
using GameResources.LevelAndScoreManagement;
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
        // Load these into the game using Resources.Load the first time around
        public GameObject ScoutBoat;
        public GameObject FisherBoat;
        public GameObject SpeedBoat;
        public GameObject JetSki;

        private List<string> _shipNames;
        private Dictionary<string, List<GameObject>> _shipPool; // We'll keep this static for each level
        private List<GameObject> _spawnedItems;
        private PooledList<IDisposable> _disposables;

        protected override void Awake()
        {
            base.Awake();
            
            if (_disposables == null)
            {
                _disposables = AppHandler.AppPool.Get<PooledList<IDisposable>>();
            }
            
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPlayToMainMenu>(OnMainMenuTransition, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerWinOrLossToMainMenu>(OnMainMenuTransition,
                _disposables);
        }

        private void OnDestroy()
        {
            _disposables.ClearDisposables();
            _disposables.ReturnToPool();
        }

        protected override void InitSingleton()
        {
            base.InitSingleton();
            _shipPool = new Dictionary<string, List<GameObject>>();
            _spawnedItems = new List<GameObject>();
            LoadShipPool();
        }

        // Make sure to name the prefabs after the variable names listed here
        // This function is a bit more manual than I'd like it to be but it'll
        // do for now
        private void LoadShipPool()
        {
            List<int> shipQuantities = new List<int>();
            LevelManager.AccessLevelData(ref shipQuantities);

            ScoutBoat = ScoutBoat == null ? AppHandler.AssetHandler.LoadAsset<GameObject>(nameof(ScoutBoat)) : ScoutBoat;
            FisherBoat = FisherBoat == null ? AppHandler.AssetHandler.LoadAsset<GameObject>(nameof(FisherBoat)) : FisherBoat;
            SpeedBoat = SpeedBoat == null ? AppHandler.AssetHandler.LoadAsset<GameObject>(nameof(SpeedBoat)) : SpeedBoat;
            JetSki = JetSki == null ? AppHandler.AssetHandler.LoadAsset<GameObject>(nameof(JetSki)) : JetSki;

            List<GameObject> shipTypes = new List<GameObject>()
            {
                ScoutBoat, FisherBoat, SpeedBoat, JetSki
            };

            _shipNames = new List<string>()
            {
                nameof(ScoutBoat), nameof(FisherBoat), nameof(SpeedBoat), nameof(JetSki)
            };

            SpawnShips(shipTypes, _shipNames, shipQuantities);
        }

        private void SpawnShips(List<GameObject> shipTypes, List<String> _shipNames, List<int> shipQuantities)
        {
            for (int i = 0; i < shipTypes.Count; i++)
            {
                List<GameObject> ships = new List<GameObject>();
                for (int j = 0; j < shipQuantities[i]; j++)
                {
                    GameObject newShip = Instantiate(shipTypes[i], transform.position, transform.rotation, transform);
                    newShip.SetActive(false);
                    ships.Add(newShip);
                }

                _shipPool.Add(_shipNames[i], ships);
            }
        }

        private void OnMainMenuTransition(REvent evt)
        {
            ResetPool();
        }

        private void ResetPool()
        {
            ClearPool();
            InitSingleton();
        }

        private void ClearPool()
        {
            for (int i = 0; i < _shipPool.Count; i++)
            {
                for (int j = 0; j < _shipPool[_shipNames[i]].Count; j++)
                {
                    Destroy(_shipPool[_shipNames[i]][j]);
                }
                _shipPool[_shipNames[i]].Clear();
            }
            _shipPool.Clear();

            for (int i = 0; i < _spawnedItems.Count; i++)
            {
                Destroy(_spawnedItems[i]);
            }
            _spawnedItems.Clear();
        }

        public GameObject Spawn(ShipTypes type, Vector3 position, Quaternion rotation)
        {
            GameObject temp = null;
            switch (type)
            {
                case ShipTypes.ScoutBoat:
                    temp = _shipPool[_shipNames[0]][0];
                    _shipPool[_shipNames[0]].RemoveAt(0);
                    break;
                case ShipTypes.FisherBoat:
                    temp = _shipPool[_shipNames[1]][0];
                    _shipPool[_shipNames[1]].RemoveAt(0);
                    break;
                case ShipTypes.SpeedBoat:
                    temp = _shipPool[_shipNames[2]][0];
                    _shipPool[_shipNames[2]].RemoveAt(0);
                    break;
                case ShipTypes.JetSki:
                    temp = _shipPool[_shipNames[3]][0];
                    _shipPool[_shipNames[3]].RemoveAt(0);
                    break;
            }
            _spawnedItems.Add(temp);
            temp.transform.position = position;
            temp.transform.rotation = rotation;
            temp.SetActive(true);
            temp.GetComponent<RPathMover>().enabled = true;
            return temp;
        }

        public void Despawn(GameObject itemToDespawn)
        {
            itemToDespawn.SetActive(false);
            itemToDespawn.transform.position = transform.position;
            itemToDespawn.transform.rotation = transform.rotation;
            switch (itemToDespawn.GetComponent<ShipController>()._shipData.ShipType)
            {
                case ShipTypes.ScoutBoat:
                    _shipPool[_shipNames[0]].Add(itemToDespawn);
                    break;
                case ShipTypes.FisherBoat:
                    _shipPool[_shipNames[1]].Add(itemToDespawn);
                    break;
                case ShipTypes.SpeedBoat:
                    _shipPool[_shipNames[2]].Add(itemToDespawn);
                    break;
                case ShipTypes.JetSki:
                    _shipPool[_shipNames[3]].Add(itemToDespawn);
                    break;
            }
        }
    }
}