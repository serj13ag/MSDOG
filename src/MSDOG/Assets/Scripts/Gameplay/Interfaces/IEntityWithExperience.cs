using System;

namespace Gameplay.Interfaces
{
    public interface IEntityWithExperience
    {
        int CurrentExperience { get; }
        int MaxExperience { get; }

        event Action OnExperienceChanged;

        void ResetExperience();
    }
}