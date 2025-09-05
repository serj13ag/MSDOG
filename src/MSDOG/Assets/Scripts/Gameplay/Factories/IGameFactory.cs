using Core.Models.Data;
using Gameplay.Enemies;
using UnityEngine;

namespace Gameplay.Factories
{
    public interface IGameFactory
    {
        IPlayer CreatePlayer();
        IEnemy CreateEnemy(Vector3 position, EnemyData data);
    }
}