using System;

namespace Core.Abilities
{
    public interface IAbility
    {
        Guid Id { get; }

        void SetId(Guid id);
        void OnUpdate(float deltaTime);
    }
}