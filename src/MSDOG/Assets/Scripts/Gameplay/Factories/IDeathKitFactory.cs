using Gameplay.Enemies;
using UnityEngine;

namespace Gameplay.Factories
{
    public interface IDeathKitFactory
    {
        void Prewarm(int levelIndex);

        EnemyDeathkit CreateDeathKit(EnemyDeathkit deathKitPrefab, Vector3 position, Quaternion rotation);
    }
}