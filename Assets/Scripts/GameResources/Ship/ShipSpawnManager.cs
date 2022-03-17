using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CoreResources.Handlers.EventHandler;
using CoreResources.Pool;
using CoreResources.Utils;
using CoreResources.Utils.Jobs;
using CoreResources.Utils.Singletons;
using GameResources.Events;
using GameResources.LevelAndScoreManagement;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameResources.Ship
{
    public class ShipSpawnManager : InitializableGenericSingleton<ShipSpawnManager>
    {
        private PooledList<GameObject> _spawnerList;
        private Dictionary<ShipTypes, int> _remainingShipsByType;
        private Dictionary<ShipColors, int> _remainingShipsByColor;
        private int _prevIndex;
        private const int randIterationCount = 5;

        private UpdateJob SpawnCoroutine;

        public Dictionary<ShipTypes, int> RemainingShipsByType => _remainingShipsByType;
        public Dictionary<ShipColors, int> RemainingShipsByColor => _remainingShipsByColor;

        public int TotalUnspawnedShips
        {
            get
            {
                int tmp = 0;
                foreach (var remainingShipTypes in _remainingShipsByType)
                {
                    tmp += remainingShipTypes.Value;
                }

                return tmp;
            }
        }

        public void AddToSpawnerList(GameObject spawnerToAdd)
        {
            if (spawnerToAdd != null)
            {
                _spawnerList.Add(spawnerToAdd);
            }
        }

        public void RemoveFromSpawnersList(GameObject spawnerToRemove)
        {
            if (_spawnerList.Contains(spawnerToRemove))
            {
                _spawnerList.Remove(spawnerToRemove);
            }
        }

        protected override void InitSingleton()
        {
            base.InitSingleton();
            _prevIndex = -1;
            _spawnerList = AppHandler.AppPool.Get<PooledList<GameObject>>();
            _remainingShipsByType = new Dictionary<ShipTypes, int>();
            GetShipLists();
            AppHandler.EventHandler.Subscribe<REvent_GameManagerMainMenuToPlay>(OnEnterPlay, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerWinOrLossToPlay>(OnEnterPlay, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPauseToPlay>(OnResumePlay, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPlayToPause>(OnExitPlay, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPlayToLoss>(OnExitPlay, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPlayToWin>(OnExitPlay, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_ShipsLoaded>(OnReset, _disposables);
        }

        protected override void CleanSingleton()
        {
            base.CleanSingleton();
            _spawnerList?.ReturnToPool();
        }

        private void OnEnterPlay(REvent evt)
        {
            GetShipLists();
        }

        private void OnResumePlay(REvent evt)
        {
            StartSpawners();
        }

        private void OnExitPlay(REvent evt)
        {
            StopSpawners();
        }
        
        private void OnReset(REvent evt)
        {
            GetShipLists();
            _prevIndex = -1;
            _spawnerList.Clear();
            ShipSpawner[] spawners = UnityEngine.Object.FindObjectsOfType<ShipSpawner>();
            foreach (var spawner in spawners)
            {
                AddToSpawnerList(spawner.gameObject);
            }
            StartSpawners();
        }

        private void GetShipLists()
        {
            LevelManager.AccessShipsByType(ref _remainingShipsByType);
            LevelManager.AccessShipsByColor(ref _remainingShipsByColor);
            int x = 0, y = 0;
            foreach (var shipType in EnumUtil.GetValues<ShipTypes>())
            {
                x += _remainingShipsByType[shipType];
            }

            foreach (var shipColor in EnumUtil.GetValues<ShipColors>())
            {
                y += _remainingShipsByColor[shipColor];
            }

            if (x != y)
                throw new ArgumentOutOfRangeException($"The total ships by color and by type donot match");
        }

        private void StartSpawners()
        {
            if (_spawnerList != null)
            {
                if (SpawnCoroutine != null)
                {
                    JobManager.SafeStopUpdate(ref SpawnCoroutine);
                }

                SpawnCoroutine = AppHandler.JobHandler.ExecuteCoroutine(SpawnIEnumerator());
            }
        }

        private void StopSpawners()
        {
            if (SpawnCoroutine != null)
            {
                JobManager.SafeStopUpdate(ref SpawnCoroutine);
            }
        }


        private IEnumerator SpawnIEnumerator()
        {
            while (SpawnableShipsExist())
            {
                yield return new WaitForSeconds(Random.Range(5, 10));
                
                int spawnerIndex = new int();
                int shipIndex = new int();
                int shipColorIndex = new int();
                GenerateRandomPermissibleIndices(ref spawnerIndex, ref shipIndex, ref shipColorIndex);
                if (shipIndex >= 0 && shipColorIndex >= 0)
                {
                    Debug.Log("ShipSpawnManager || Ship spawned!");
                    _spawnerList[spawnerIndex].GetComponent<ShipSpawner>().Spawn((ShipTypes) shipIndex, (ShipColors) shipColorIndex);
                    _remainingShipsByType[(ShipTypes) shipIndex]--;
                    _remainingShipsByColor[(ShipColors) shipColorIndex]--;
                }
            }
        }

        private bool SpawnableShipsExist()
        {
            foreach (var shipType in EnumUtil.GetValues<ShipTypes>())
            {
                if (_remainingShipsByType[shipType] > 0)
                {
                    return true;
                }
            }
            
            return false;
        }

        // Check if the random value isn't the same as the previous
        // index and it's not pointing to an emptied list
        private void GenerateRandomPermissibleIndices(ref int spawnerIndex, ref int shipIndex, ref int colorIndex)
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            spawnerIndex = Random.Range(0, _spawnerList.Count);
            if (spawnerIndex == _prevIndex)
            {
                if (spawnerIndex == _spawnerList.Count - 1)
                {
                    spawnerIndex--;
                }
                else
                {
                    spawnerIndex++;
                }
            }

            _prevIndex = spawnerIndex;

            // Check if this random value points to an empty list and
            // then just try to find a full list
            shipIndex = Random.Range(0, _remainingShipsByType.Count);
            if (_remainingShipsByType[(ShipTypes) shipIndex] < 1)
            {
                int tmp = shipIndex;
                for (int i = 0; i < randIterationCount; i++)
                {
                    int rndIndex = Random.Range(0, _remainingShipsByType.Count);
                    if (_remainingShipsByType[(ShipTypes) rndIndex] > 0)
                    {
                        shipIndex = rndIndex;
                        break;
                    }
                }

                if(tmp == shipIndex)
                    shipIndex = -1;
            }

            colorIndex = Random.Range(0, _remainingShipsByColor.Count);
            if (_remainingShipsByColor[(ShipColors) colorIndex] < 1)
            {
                int tmp = colorIndex;
                for (int i = 0; i < randIterationCount; i++)
                {
                    int rndIndex = Random.Range(0, _remainingShipsByColor.Count);
                    if (_remainingShipsByColor[(ShipColors) rndIndex] > 0)
                    {
                        colorIndex = rndIndex;
                        break;
                    }
                }

                if (tmp == colorIndex)
                    colorIndex = -1;
            }
        }
    }
}