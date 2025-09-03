using System;
using System.Collections.Generic;
using Core.Models.Data;
using Core.Services;
using Gameplay.Controllers;
using Gameplay.Enemies;
using Gameplay.Factories;
using Gameplay.Interfaces;
using Gameplay.Providers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Services
{
    public class EnemyService : IEnemyService, IUpdatable, IDisposable
    {
        private const int MaxSpawnAttempts = 30;
        private const float MinDistanceFromPlayer = 4f;
        private const float MaxDistanceFromPlayer = 12f;
        private const float MinDistanceBetweenEnemies = 1f;

        private readonly IGameFactory _gameFactory;
        private readonly IArenaService _arenaService;
        private readonly IGameplayUpdateController _updateController;
        private readonly IDataService _dataService;
        private readonly IDebugController _debugController;
        private readonly IDeathKitFactory _deathKitFactory;
        private readonly IPlayerProvider _playerProvider;

        private bool _isActive;
        private List<WaveData> _waves;
        private int _nextWaveIndex;
        private float _timeTillSpawnNextWave;

        private readonly List<IEnemy> _enemies = new List<IEnemy>();

        public event Action OnAllEnemiesDied;

        public EnemyService(IGameplayUpdateController updateController, IDataService dataService, IGameFactory gameFactory,
            IArenaService arenaService, IDebugController debugController, IDeathKitFactory deathKitFactory,
            IPlayerProvider playerProvider)
        {
            _debugController = debugController;
            _deathKitFactory = deathKitFactory;
            _playerProvider = playerProvider;
            _gameFactory = gameFactory;
            _arenaService = arenaService;
            _updateController = updateController;
            _dataService = dataService;

            updateController.Register(this);

            debugController.OnForceSpawnEnemiesRequested += OnForceSpawnEnemiesRequested;
            debugController.OnKillAllEnemiesRequested += OnKillAllEnemiesRequested;
        }

        public void InitLevel(int levelIndex)
        {
            _waves = _dataService.GetLevelData(levelIndex).Waves;
        }

        public void ActivateLevel()
        {
            _isActive = true;
        }

        public void OnUpdate(float deltaTime)
        {
            if (!_isActive)
            {
                return;
            }

            if (_timeTillSpawnNextWave > 0f)
            {
                _timeTillSpawnNextWave -= deltaTime;
                return;
            }

            SpawnWave(_nextWaveIndex);

            _timeTillSpawnNextWave = _waves[_nextWaveIndex].TimeTillNextWave;
            _nextWaveIndex++;

            if (_nextWaveIndex >= _waves.Count)
            {
                _isActive = false;
            }
        }

        private void OnForceSpawnEnemiesRequested(object sender, EventArgs e)
        {
            _timeTillSpawnNextWave = 0f;
        }

        private void OnKillAllEnemiesRequested(object sender, EventArgs e)
        {
            foreach (var enemy in _enemies.ToArray())
            {
                enemy.Kill();
            }
        }

        private void SpawnWave(int waveIndex)
        {
            var waveData = _waves[waveIndex];

            var spawnedEnemyPositions = new List<Vector3>();

            foreach (var enemyWaveData in waveData.Enemies)
            {
                for (var i = 0; i < enemyWaveData.Count; i++)
                {
                    var position = FindValidSpawnPosition(spawnedEnemyPositions);
                    spawnedEnemyPositions.Add(position);

                    var enemy = _gameFactory.CreateEnemy(position, enemyWaveData.Data);
                    _enemies.Add(enemy);

                    enemy.OnDied += OnEnemyDied;
                }
            }
        }

        private void OnEnemyDied(IEnemy enemy)
        {
            _enemies.Remove(enemy);
            enemy.OnDied -= OnEnemyDied;

            SpawnDeathKit(enemy);

            if (!_isActive && _enemies.Count == 0)
            {
                OnAllEnemiesDied?.Invoke();
            }
        }

        private Vector3 FindValidSpawnPosition(List<Vector3> spawnedEnemyPositions)
        {
            for (var attempt = 0; attempt < MaxSpawnAttempts; attempt++)
            {
                var randomPosition = GetRandomPositionInArena();
                if (IsPositionValid(randomPosition, spawnedEnemyPositions))
                {
                    return randomPosition;
                }
            }

            return Vector3.zero;
        }

        private Vector3 GetRandomPositionInArena()
        {
            var x = Random.Range(-_arenaService.HalfSize.X, _arenaService.HalfSize.X);
            var z = Random.Range(-_arenaService.HalfSize.Y, _arenaService.HalfSize.Y);
            return new Vector3(x, 0f, z);
        }

        private bool IsPositionValid(Vector3 position, List<Vector3> spawnedEnemyPositions)
        {
            var distanceToPlayer = Vector3.Distance(position, _playerProvider.Player.transform.position);
            if (distanceToPlayer < MinDistanceFromPlayer ||
                distanceToPlayer > MaxDistanceFromPlayer)
            {
                return false;
            }

            foreach (var spawnedEnemyPosition in spawnedEnemyPositions)
            {
                var distance = Vector3.Distance(position, spawnedEnemyPosition);
                if (distance < MinDistanceBetweenEnemies)
                {
                    return false;
                }
            }

            return true;
        }

        private void SpawnDeathKit(IEnemy enemy)
        {
            _deathKitFactory.CreateDeathKit(enemy.DeathkitPrefab, enemy.ModelRootPosition, enemy.GetRotation());
        }

        public void Dispose()
        {
            _updateController.Remove(this);

            _debugController.OnForceSpawnEnemiesRequested += OnForceSpawnEnemiesRequested;
            _debugController.OnKillAllEnemiesRequested += OnKillAllEnemiesRequested;
        }
    }
}