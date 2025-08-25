using System;
using Gameplay.Controllers;
using UnityEngine;
using UnityEngine.AI;
using VContainer;

namespace Utility
{
    public class NavMeshAgentSpeedChangeComponent : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;

        private IGameplayUpdateController _updateController;

        [Inject]
        public void Construct(IGameplayUpdateController updateController)
        {
            _updateController = updateController;
        }

        private void OnEnable()
        {
            _updateController.OnGameTimeChanged += OnGameTimeChanged;
        }

        private void OnGameTimeChanged(object sender, EventArgs e)
        {
            _navMeshAgent.isStopped = _updateController.IsPaused;
        }

        private void OnDisable()
        {
            _updateController.OnGameTimeChanged -= OnGameTimeChanged;
        }
    }
}