using Core.Models.Data;
using Gameplay.Enemies;
using UnityEngine;

namespace Gameplay.Factories
{
    public interface IGameFactory
    {
        Player CreatePlayer();
        IEnemy CreateEnemy(Vector3 position, EnemyData data);
    }
}