using System;
using UnityEngine;

namespace Gameplay.Services
{
    public class GameSpeedService : IGameSpeedService
    {
        public float GameSpeed { get; private set; } = 1f;
        public bool IsPaused => GameSpeed == 0f;

        public event EventHandler<EventArgs> OnGameTimeChanged;

        public void Pause()
        {
            GameSpeed = 0f;
            Physics.simulationMode = SimulationMode.Script;

            OnGameTimeChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Unpause()
        {
            GameSpeed = 1f;
            Physics.simulationMode = SimulationMode.FixedUpdate;

            OnGameTimeChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}