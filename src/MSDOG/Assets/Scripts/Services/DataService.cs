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
        private readonly LevelAbilityData _levelAbilityData;

        public DataService()
        {
            _levelsData = Resources.LoadAll<LevelData>(AssetPaths.LevelsData).ToDictionary(k => k.LevelIndex);
            _levelAbilityData = Resources.Load<LevelAbilityData>(AssetPaths.LevelAbilityData);
        }

        public LevelData GetLevelData(int levelIndex)
        {
            return _levelsData.GetValueOrDefault(levelIndex);
        }

        public AbilityData GetStartAbilityData()
        {
            return _levelAbilityData.StartAbility;
        }

        public AbilityData GetRandomCraftAbilityData()
        {
            var randomIndex = Random.Range(0, _levelAbilityData.AbilitiesAvailableToCraft.Count);
            return _levelAbilityData.AbilitiesAvailableToCraft[randomIndex];
        }
    }
}