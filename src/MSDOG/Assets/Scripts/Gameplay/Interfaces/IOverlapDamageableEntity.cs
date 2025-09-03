using System;

namespace Gameplay.Interfaces
{
    public interface IOverlapDamageableEntity
    {
        void RegisterOverlapDamager(Guid damagerId, int damage);
        void RemoveOverlapDamager(Guid damagerId);
    }
}