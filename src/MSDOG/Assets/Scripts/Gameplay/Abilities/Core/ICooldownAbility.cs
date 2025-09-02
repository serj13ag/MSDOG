using System;

namespace Gameplay.Abilities.Core
{
    public interface ICooldownAbility : IAbility
    {
        event Action OnActionInvoked;
    }
}