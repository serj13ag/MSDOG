using UnityEngine;

namespace Gameplay.Factories
{
    public interface IDamageTextFactory
    {
        void CreateDamageTextEffect(int damageDealt, Vector3 position);
    }
}