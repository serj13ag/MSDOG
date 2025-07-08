using System.Collections.Generic;
using Data;
using Interfaces;
using UnityEngine;

namespace Services.Gameplay
{
    public class EnemyService : IUpdatable
    {
        private readonly GameFactory _gameFactory;
        private readonly ArenaService _arenaService;
        private readonly UpdateService _updateService;
        private readonly DataService _dataService;

        private bool _isActive;
        private List<WaveData> _waves;
        private int _nextWaveIndex;
        private float _timeTillSpawnNextWave;

        public EnemyService(UpdateService updateService, DataService dataService, GameFactory gameFactory,
            ArenaService arenaService)
        {
            _gameFactory = gameFactory;
            _arenaService = arenaService;
            _updateService = updateService;
            _dataService = dataService;

            updateService.Register(this);
        }

        public void ActivateLevel(int levelIndex)
        {
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
            for (var i = 0; i < waveData.Enemies; i++)
            {
                var position = new Vector3(i, 0f, i); // TODO: add positioning
                _gameFactory.CreateEnemy(position);
            }
        }

        public void Cleanup()
        {
            _updateService.Remove(this);
        }
    }
}