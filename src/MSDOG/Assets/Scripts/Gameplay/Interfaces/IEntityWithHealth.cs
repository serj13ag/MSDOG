using System;

namespace Gameplay.Interfaces
{
    public interface IEntityWithHealth
    {
        bool IsFullHealth { get; }
        int CurrentHealth { get; }
        int MaxHealth { get; }

        event Action OnHealthChanged;
    }
}