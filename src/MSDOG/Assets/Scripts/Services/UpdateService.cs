using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Services
{
    public class UpdateService : BaseMonoService
    {
        private readonly List<IUpdatable> _updatables = new List<IUpdatable>();

        private void Update()
        {
            foreach (var updatable in _updatables)
            {
                updatable.OnUpdate(Time.deltaTime);
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
    }
}