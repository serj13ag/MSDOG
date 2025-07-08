using Interfaces;
using Services;
using Services.Gameplay;
using UnityEngine;

namespace Core
{
    public class Enemy : MonoBehaviour, IUpdatable
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float _moveSpeed;

        private UpdateService _updateService;
        private ArenaService _arenaService;

        public void Init(UpdateService updateService, ArenaService arenaService)
        {
            _arenaService = arenaService;
            _updateService = updateService;

            updateService.Register(this);
        }

        public void OnUpdate(float deltaTime)
        {
        }

        private void OnDestroy()
        {
            _updateService.Remove(this);
        }
    }
}