using System;
using System.Collections.Generic;
using Core.Controllers;
using Core.Interfaces;
using Core.Models.Data;
using Core.Services;
using Gameplay.Controllers;
using Gameplay.Enemies;
using Gameplay.Factories;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Gameplay.Services
{
    public class EnemyService : IUpdatable, IDisposable
    {
        private const int MaxSpawnAttempts = 30;
        private const float MinDistanceFromPlayer = 4f;
        private const float MaxDistanceFromPlayer = 12f;
        private const float MinDistanceBetweenEnemies = 1f;

        private readonly GameFactory _gameFactory;
        private readonly ArenaService _arenaService;
        private readonly IUpdateController _updateController;
        private readonly IDataService _dataService;
        private readonly DebugController _debugController;
        private readonly DeathKitFactory _deathKitFactory;

        private bool _isActive;
        private Transform _playerTransform;
        private List<WaveData> _waves;
        private int _nextWaveIndex;
        private float _timeTillSpawnNextWave;

        private readonly List<Enemy> _enemies = new List<Enemy>();

        public event Action OnAllEnemiesDied;

        public EnemyService(IUpdateController updateController, IDataService dataService, GameFactory gameFactory,
            ArenaService arenaService, DebugController debugController, DeathKitFactory deathKitFactory)
        {
            _debugController = debugController;
            _deathKitFactory = deathKitFactory;
            _gameFactory = gameFactory;
            _arenaService = arenaService;
            _updateController = updateController;
            _dataService = dataService;

            updateController.Register(this);

            debugController.OnForceSpawnEnemiesRequested += OnForceSpawnEnemiesRequested;
            debugController.OnKillAllEnemiesRequested += OnKillAllEnemiesRequested;
        }

        public void InitLevel(int levelIndex, Transform playerTransform)
        {
            _playerTransform = playerTransform;
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

        private void OnEnemyDied(Enemy enemy)
        {
            _enemies.Remove(enemy);
            enemy.OnDied -= OnEnemyDied;

            SpawnDeathKit(enemy);
            Object.Destroy(enemy.gameObject); // TODO: pool?

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
            if (_playerTransform)
            {
                var distanceToPlayer = Vector3.Distance(position, _playerTransform.position);
                if (distanceToPlayer < MinDistanceFromPlayer ||
                    distanceToPlayer > MaxDistanceFromPlayer)
                {
                    return false;
                }
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

        private void SpawnDeathKit(Enemy enemy)
        {
            _deathKitFactory.CreateDeathKit(enemy.DeathkitPrefab, enemy.ModelRootPosition, enemy.transform.rotation);
        }

        public void Dispose()
        {
            _updateController.Remove(this);

            _debugController.OnForceSpawnEnemiesRequested += OnForceSpawnEnemiesRequested;
            _debugController.OnKillAllEnemiesRequested += OnKillAllEnemiesRequested;
        }
    }
}