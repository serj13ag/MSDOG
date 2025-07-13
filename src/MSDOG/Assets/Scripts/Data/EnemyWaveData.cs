using System;
using Core.Enemies;

namespace Data
{
    [Serializable]
    public class EnemyWaveData
    {
        public EnemyType Type;
        public int Count;
    }
}