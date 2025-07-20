using System.Collections.Generic;
using System.Linq;
using Constants;
using Core.Abilities;
using Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace Services
{
    public class DataService
    {
        private readonly Dictionary<int, LevelData> _levelsData;
        private readonly LevelAbilityData _levelAbilityData;
        private readonly AbilityUpgradesData _abilityUpgradesData;

        public DataService()
        {
            _levelsData = Resources.LoadAll<LevelData>(AssetPaths.LevelsData).ToDictionary(k => k.LevelIndex);
            for (var i = 0; i < _levelsData.Count; i++)
            {
                Assert.IsTrue(_levelsData.ContainsKey(i));
            }

            _levelAbilityData = Resources.Load<LevelAbilityData>(AssetPaths.LevelAbilityData);
            _abilityUpgradesData = Resources.Load<AbilityUpgradesData>(AssetPaths.AbilityUpgradesData);
        }

        public int GetNumberOfLevels()
        {
            return _levelsData.Count;
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

        public bool TryGetAbilityUpgradeData(AbilityType abilityType, int level, out AbilityData upgradedAbilityData)
        {
            upgradedAbilityData = null;

            foreach (var abilityUpgradeEntry in _abilityUpgradesData.AbilityUpgrades)
            {
                if (abilityUpgradeEntry.AbilityType != abilityType)
                {
                    continue;
                }

                foreach (var abilityUpgrade in abilityUpgradeEntry.AbilityUpgrades)
                {
                    if (abilityUpgrade.Level != level + 1)
                    {
                        continue;
                    }

                    upgradedAbilityData = abilityUpgrade;
                    return true;
                }
            }

            return false;
        }
    }
}