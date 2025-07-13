using System.Collections.Generic;
using Core.Enemies;
using Data;
using Infrastructure;
using Infrastructure.StateMachine;
using Interfaces;
using UnityEngine;

namespace Services.Gameplay
{
    public class EnemyService : IUpdatable
    {
        private const int MaxSpawnAttempts = 30;
        private const float MinDistanceFromPlayer = 4f;
        private const float MinDistanceBetweenEnemies = 2f;

        private readonly GameFactory _gameFactory;
        private readonly ArenaService _arenaService;
        private readonly UpdateService _updateService;
        private readonly DataService _dataService;

        private bool _isActive;
        private Transform _playerTransform;
        private List<WaveData> _waves;
        private int _nextWaveIndex;
        private float _timeTillSpawnNextWave;

        private readonly List<Enemy> _enemies = new List<Enemy>();

        public EnemyService(UpdateService updateService, DataService dataService, GameFactory gameFactory,
            ArenaService arenaService)
        {
            _gameFactory = gameFactory;
            _arenaService = arenaService;
            _updateService = updateService;
            _dataService = dataService;

            updateService.Register(this);
        }

        public void ActivateLevel(int levelIndex, Transform playerTransform)
        {
            _playerTransform = playerTransform;
            _waves = _dataService.GetLevelData(levelIndex).Waves;
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

            Object.Destroy(enemy.gameObject); // TODO: pool?

            if (!_isActive && _enemies.Count == 0)
            {
                GlobalServices.GameStateMachine.Enter<GameplayState>(); // TODO: show window
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
                if (distanceToPlayer < MinDistanceFromPlayer)
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

        public void Cleanup()
        {
            _updateService.Remove(this);
        }
    }
}