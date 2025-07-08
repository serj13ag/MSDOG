using System.Collections.Generic;
using System.Linq;
using Constants;
using Data;
using UnityEngine;

namespace Services
{
    public class DataService
    {
        private readonly Dictionary<int, LevelData> _levelsData;

        public DataService()
        {
            _levelsData = Resources.LoadAll<LevelData>(AssetPaths.LevelsData).ToDictionary(k => k.LevelIndex);
        }

        public LevelData GetLevelData(int levelIndex)
        {
            return _levelsData.GetValueOrDefault(levelIndex);
        }
    }
}