using System;
using System.Collections.Generic;
using Core.Interfaces;
using UnityEngine;

namespace Core.Controllers
{
    public class UpdateController : BasePersistentController, IUpdateController
    {
        private readonly List<IUpdatable> _updatables = new List<IUpdatable>();
        private readonly Queue<IUpdatable> _toAdd = new Queue<IUpdatable>();
        private readonly Queue<IUpdatable> _toRemove = new Queue<IUpdatable>();

        private float _gameTimeScale = 1f;

        public bool IsPaused => _gameTimeScale == 0f;
        public float GameTimeScale => _gameTimeScale;

        public event EventHandler<EventArgs> OnGameTimeChanged;

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

            var deltaTime = Time.deltaTime * _gameTimeScale;
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

        public void Pause()
        {
            _gameTimeScale = 0f;
            Physics.simulationMode = SimulationMode.Script;

            // TODO: fix animator components
            // TODO: fix using unity update
            // TODO: separate update service from game speed service
            OnGameTimeChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Unpause()
        {
            _gameTimeScale = 1f;
            Physics.simulationMode = SimulationMode.FixedUpdate;

            OnGameTimeChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}