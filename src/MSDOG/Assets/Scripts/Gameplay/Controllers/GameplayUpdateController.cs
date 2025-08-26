using System.Collections.Generic;
using Gameplay.Interfaces;
using Gameplay.Services;
using UnityEngine;
using VContainer;

namespace Gameplay.Controllers
{
    public class GameplayUpdateController : MonoBehaviour, IGameplayUpdateController
    {
        private readonly List<IUpdatable> _updatables = new List<IUpdatable>();
        private readonly Queue<IUpdatable> _toAdd = new Queue<IUpdatable>();
        private readonly Queue<IUpdatable> _toRemove = new Queue<IUpdatable>();

        private IGameSpeedService _gameSpeedService;

        [Inject]
        public void Construct(IGameSpeedService gameSpeedService)
        {
            _gameSpeedService = gameSpeedService;
        }

        private void Update()
        {
            while (_toRemove.Count > 0)
            {
                _updatables.Remove(_toRemove.Dequeue());
            }

            while (_toAdd.Count > 0)
            {
                var updatableToAdd = _toAdd.Dequeue();
                if (_updatables.Contains(updatableToAdd))
                {
                    Debug.LogError($"Trying to add already registered updatable: {updatableToAdd.GetType()}!");
                }
                else
                {
                    _updatables.Add(updatableToAdd);
                }
            }

            var deltaTime = Time.deltaTime * _gameSpeedService.GameSpeed;
            foreach (var updatable in _updatables)
            {
                updatable.OnUpdate(deltaTime);
            }
        }

        public void Register(IUpdatable updatable)
        {
            _toAdd.Enqueue(updatable);
        }

        public void Remove(IUpdatable updatable)
        {
            _toRemove.Enqueue(updatable);
        }
    }
}