using Core.Models.Data;
using Gameplay.Enemies;
using UnityEngine;

namespace Gameplay.Factories
{
    public interface IGameFactory
    {
        Player CreatePlayer();
        Enemy CreateEnemy(Vector3 position, EnemyData data);
        ExperiencePiece CreateExperiencePiece(Vector3 position, int experience);
    }
}