using System;
using Core.Controllers;
using UnityEngine;
using UnityEngine.AI;
using VContainer;

namespace Utility
{
    public class NavMeshAgentSpeedChangeComponent : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;

        private IUpdateController _updateController;

        [Inject]
        public void Construct(IUpdateController updateController)
        {
            _updateController = updateController;

            updateController.OnGameTimeChanged += OnGameTimeChanged;
        }

        private void OnGameTimeChanged(object sender, EventArgs e)
        {
            _navMeshAgent.isStopped = _updateController.IsPaused;
        }

        private void OnDestroy()
        {
            _updateController.OnGameTimeChanged -= OnGameTimeChanged;
        }
    }
}