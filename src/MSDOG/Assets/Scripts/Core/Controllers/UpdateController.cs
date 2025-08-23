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

        public void Pause(bool withTimeScale = false)
        {
            _gameTimeScale = 0f;

            if (withTimeScale)
            {
                Time.timeScale = 0f; // TODO: fix curtain
            }
        }

        public void Unpause(bool withTimeScale = false)
        {
            _gameTimeScale = 1f;

            if (withTimeScale)
            {
                Time.timeScale = 1f;
            }
        }
    }
}