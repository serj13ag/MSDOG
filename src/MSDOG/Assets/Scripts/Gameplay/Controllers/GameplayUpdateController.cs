using System.Collections.Generic;
using Gameplay.Interfaces;
using Gameplay.Services;
using UnityEngine;
using VContainer;

namespace Gameplay.Controllers
{
    public class GameplayUpdateController : MonoBehaviour, IGameplayUpdateController
    {
        private readonly HashSet<IUpdatable> _updatables = new HashSet<IUpdatable>();
        private readonly HashSet<IUpdatable> _toAdd = new HashSet<IUpdatable>();
        private readonly HashSet<IUpdatable> _toRemove = new HashSet<IUpdatable>();

        private IGameSpeedService _gameSpeedService;

        [Inject]
        public void Construct(IGameSpeedService gameSpeedService)
        {
            _gameSpeedService = gameSpeedService;
        }

        private void Update()
        {
            foreach (var updatableToRemove in _toRemove)
            {
                _toAdd.Remove(updatableToRemove);
                _updatables.Remove(updatableToRemove);
            }
            _toRemove.Clear();

            foreach (var updatableToAdd in _toAdd)
            {
                if (!_updatables.Add(updatableToAdd))
                {
                    Debug.LogError($"Trying to add already registered updatable: {updatableToAdd.GetType()}!");
                }
            }
            _toAdd.Clear();

            var deltaTime = Time.deltaTime * _gameSpeedService.GameSpeed;
            foreach (var updatable in _updatables)
            {
                updatable.OnUpdate(deltaTime);
            }
        }

        public void Register(IUpdatable updatable)
        {
            _toAdd.Add(updatable);
        }

        public void Remove(IUpdatable updatable)
        {
            _toRemove.Add(updatable);
        }
    }
}