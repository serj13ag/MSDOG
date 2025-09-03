using System;

namespace Gameplay.Interfaces
{
    public interface IProjectileDamageableEntity
    {
        void TakeProjectileDamage(Guid projectileId, int damage);
    }
}