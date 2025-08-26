using System;
using Gameplay.Services;
using UnityEngine;
using UnityEngine.AI;
using VContainer;

namespace Utility
{
    public class NavMeshAgentSpeedChangeComponent : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;

        private IGameSpeedService _gameSpeedService;

        [Inject]
        public void Construct(IGameSpeedService gameSpeedService)
        {
            _gameSpeedService = gameSpeedService;
        }

        private void OnEnable()
        {
            _gameSpeedService.OnGameTimeChanged += OnGameTimeChanged;
        }

        private void OnGameTimeChanged(object sender, EventArgs e)
        {
            _navMeshAgent.isStopped = _gameSpeedService.IsPaused;
        }

        private void OnDisable()
        {
            _gameSpeedService.OnGameTimeChanged -= OnGameTimeChanged;
        }
    }
}