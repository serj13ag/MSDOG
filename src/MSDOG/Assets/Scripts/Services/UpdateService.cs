using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Services
{
    public class UpdateService : BaseMonoService
    {
        private readonly List<IUpdatable> _updatables = new List<IUpdatable>();

        private float _gameTime = 1f;

        public bool IsPaused => _gameTime == 0f;

        private void Update()
        {
            foreach (var updatable in _updatables.ToArray())
            {
                updatable.OnUpdate(Time.deltaTime * _gameTime);
            }
        }

        public void Register(IUpdatable updatable)
        {
            _updatables.Add(updatable);
        }

        public void Remove(IUpdatable updatable)
        {
            _updatables.Remove(updatable);
        }

        public void Pause()
        {
            _gameTime = 0f;
        }

        public void Unpause()
        {
            _gameTime = 1f;
        }
    }
}