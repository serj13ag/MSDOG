using System.Collections.Generic;
using System.Linq;
using Constants;
using Core.Models.Data;
using Core.Sounds;
using Gameplay.Abilities;
using UnityEngine;
using UnityEngine.Assertions;

namespace Core.Services
{
    public class DataService
    {
        private readonly Dictionary<int, LevelData> _levelsData;

        private readonly AbilityData[] _abilitiesData;

        //private readonly LevelAbilityData _levelAbilityData;
        private readonly AbilityUpgradesData _abilityUpgradesData;
        private readonly SettingsData _settingsData;
        private readonly SoundSettingsData _soundSettingsData;

        public DataService()
        {
            _levelsData = Resources.LoadAll<LevelData>(AssetPaths.LevelsData).ToDictionary(k => k.LevelIndex);
            for (var i = 0; i < _levelsData.Count; i++)
            {
                Assert.IsTrue(_levelsData.ContainsKey(i));
            }

            _abilitiesData = Resources.LoadAll<AbilityData>(AssetPaths.AbilitiesData);
            //_levelAbilityData = Resources.Load<LevelAbilityData>(AssetPaths.LevelAbilityData);
            _abilityUpgradesData = Resources.Load<AbilityUpgradesData>(AssetPaths.AbilityUpgradesData);
            _settingsData = Resources.Load<SettingsData>(AssetPaths.SettingsData);
            _soundSettingsData = Resources.Load<SoundSettingsData>(AssetPaths.SoundSettingsData);
        }

        public int GetNumberOfLevels()
        {
            return _levelsData.Count;
        }

        public LevelData GetLevelData(int levelIndex)
        {
            return _levelsData.GetValueOrDefault(levelIndex);
        }

        public List<AbilityData> GetStartAbilitiesData(int levelIndex)
        {
            var levelData = _levelsData.GetValueOrDefault(levelIndex);
            return levelData?.LevelAbilityData.StartAbilities;
        }

        public List<AbilityData> GetAbilitiesAvailableToCraft(int levelIndex)
        {
            var levelData = _levelsData.GetValueOrDefault(levelIndex);
            return levelData?.LevelAbilityData.AbilitiesAvailableToCraft;
        }

        public AbilityData GetRandomCraftAbilityData(int levelIndex)
        {
            var levelData = _levelsData.GetValueOrDefault(levelIndex);
            var abilitiesAvailableToCraft = levelData.LevelAbilityData.AbilitiesAvailableToCraft;
            var randomIndex = Random.Range(0, abilitiesAvailableToCraft.Count);
            return abilitiesAvailableToCraft[randomIndex];
        }

        public IEnumerable<AbilityData> GetAbilitiesData()
        {
            foreach (var abilityData in _abilitiesData)
            {
                yield return abilityData;
            }
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

        public SettingsData GetSettingsData()
        {
            return _settingsData;
        }

        public SoundClip GetEffectSoundClip(SfxType sfxType)
        {
            return _soundSettingsData.Effects.SingleOrDefault(x => x.Type == sfxType)?.AudioClip;
        }

        public SoundSettingsData GetSoundSettingsData()
        {
            return _soundSettingsData;
        }

        public ProjectileData GetEnemyProjectileData()
        {
            return _settingsData.EnemyProjectileData;
        }
    }
}