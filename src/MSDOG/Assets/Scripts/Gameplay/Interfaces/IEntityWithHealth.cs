using System;

namespace Gameplay.Interfaces
{
    public interface IEntityWithHealth
    {
        int CurrentHealth { get; }
        int MaxHealth { get; }

        event Action OnHealthChanged;
    }
}