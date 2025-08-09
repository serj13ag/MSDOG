using System;
using UnityEngine;

namespace Gameplay.Services
{
    public interface IEnemyService
    {
        event Action OnAllEnemiesDied;

        void InitLevel(int levelIndex);
        void ActivateLevel();
    }
}