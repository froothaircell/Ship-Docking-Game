using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CoreResources.Handlers.EventHandler;
using CoreResources.Utils.Jobs;
using CoreResources.Utils.Singletons;
using GameResources.Events;
using GameResources.LevelAndScoreManagement;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameResources.Ship
{
    public class ShipSpawnManager : GenericSingleton<ShipSpawnManager>
    {
        private List<GameObject> _spawnerList;
        private List<IDisposable> _disposables;
        private List<int> _remainingShips;
        private int _prevIndex;

        private UpdateJob SpawnCoroutine;

        public List<int> RemainingShips => _remainingShips;

        public int RemainingShipsTotal
        {
            get
            {
                int tmp = 0;
                foreach (var remainingShipTypes in _remainingShips)
                {
                    tmp += remainingShipTypes;
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
            _disposables = new List<IDisposable>();
            _spawnerList = new List<GameObject>();
            _remainingShips = new List<int>();
            _prevIndex = -1;
            LevelManager.AccessLevelData(ref _remainingShips);
            AppHandler.EventHandler.Subscribe<REvent_LevelStart>(OnReset, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerMainMenuToPlay>(OnEnterPlay, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerWinOrLossToPlay>(OnEnterPlay, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPauseToPlay>(OnResumePlay, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPlayToPause>(OnExitPlay, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPlayToLoss>(OnExitPlay, _disposables);
            AppHandler.EventHandler.Subscribe<REvent_GameManagerPlayToWin>(OnExitPlay, _disposables);
        }

        private void OnEnterPlay(REvent evt)
        {
            LevelManager.AccessLevelData(ref _remainingShips);
        }

        private void OnResumePlay(REvent evt)
        {
            StartSpawners();
        }

        private void OnExitPlay(REvent evt)
        {
            StopSpawners();
        }

        private void StartSpawners()
        {
            if (_spawnerList != null)
            {
                if (SpawnCoroutine != null)
                {
                    JobManager.SafeStopUpdate(ref SpawnCoroutine);
                }

                AppHandler.JobHandler.ExecuteCoroutine(SpawnIEnumerator());
            }
        }

        private void StopSpawners()
        {
            if (SpawnCoroutine != null)
            {
                JobManager.SafeStopUpdate(ref SpawnCoroutine);
            }
        }

        protected override void OnReset(REvent evt)
        {
                LevelManager.AccessLevelData(ref _remainingShips);
            _prevIndex = -1;
            ShipSpawner[] spawners = FindObjectsOfType<ShipSpawner>();
            foreach (var spawner in spawners)
            {
                AddToSpawnerList(spawner.gameObject);
            }
            StartSpawners();
        }

        private IEnumerator SpawnIEnumerator()
        {
            while (SpawnableShipsExist())
            {
                int spawnerIndex = new int();
                int shipIndex = new int();
                GenerateRandomPermissibleIndices(ref spawnerIndex, ref shipIndex);
                if (shipIndex >= 0)
                {
                    _spawnerList[spawnerIndex].GetComponent<ShipSpawner>().Spawn((ShipTypes) shipIndex);
                    _remainingShips[shipIndex]--;
                }
                yield return new WaitForSeconds(Random.Range(5, 10));
            }
        }

        private bool SpawnableShipsExist()
        {
            for (int i = 0; i < _remainingShips.Count; i++)
            {
                if (_remainingShips[i] > 0)
                {
                    return true;
                }
            }

            return false;
        }

        // Check if the random value isn't the same as the previous
        // index and it's not pointing to an emptied list
        private void GenerateRandomPermissibleIndices(ref int spawnerIndex, ref int shipIndex)
        {
            spawnerIndex = Random.Range(0, _spawnerList.Count - 1);
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
            shipIndex = Random.Range(0, _remainingShips.Count - 1);
            if (_remainingShips[shipIndex] <= 0)
            {
                int tmp = shipIndex;
                for (int i = 0; i < _remainingShips.Count; i++)
                {
                    if (_remainingShips[i] > 0)
                    {
                        shipIndex = i;
                    }
                }

                if(tmp == shipIndex)
                    shipIndex = -1;
            }
        }
    }
}