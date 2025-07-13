using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class WaveData
    {
        public List<EnemyWaveData> Enemies;
        public float TimeTillNextWave;
    }
}