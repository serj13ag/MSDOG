using System;
using System.Collections.Generic;

namespace Core.Models.Data
{
    [Serializable]
    public class WaveData
    {
        public List<EnemyWaveData> Enemies;
        public float TimeTillNextWave;
    }
}