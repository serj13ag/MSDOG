using System.Collections.Generic;
using System.Linq;
using Core.Models.Data;
using Core.Sounds;
using Gameplay.Abilities;
using UnityEngine;
using UnityEngine.Assertions;

namespace Core.Services
{
    public class DataService : IDataService
    {
        private const string LevelsDataPath = "Data/Levels";
        private const string AbilitiesDataPath = "Data/Abilities";
        private const string AbilityUpgradesDataPath = "Data/AbilityUpgradesData";
        private const string SettingsDataPath = "Data/SettingsData";
        private const string SoundSettingsDataPath = "Data/SoundSettingsData";

        private readonly Dictionary<int, LevelData> _levelsData;

        private readonly AbilityData[] _abilitiesData;

        private readonly AbilityUpgradesData _abilityUpgradesData;
        private readonly SettingsData _settingsData;
        private readonly SoundSettingsData _soundSettingsData;

        public DataService()
        {
            _levelsData = Resources.LoadAll<LevelData>(LevelsDataPath).ToDictionary(k => k.LevelIndex);
            for (var i = 0; i < _levelsData.Count; i++)
            {
                Assert.IsTrue(_levelsData.ContainsKey(i));
            }

            _abilitiesData = Resources.LoadAll<AbilityData>(AbilitiesDataPath);
            _abilityUpgradesData = Resources.Load<AbilityUpgradesData>(AbilityUpgradesDataPath);
            _settingsData = Resources.Load<SettingsData>(SettingsDataPath);
            _soundSettingsData = Resources.Load<SoundSettingsData>(SoundSettingsDataPath);
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

        public SettingsData GetSettings()
        {
            return _settingsData;
        }

        public SoundSettingsData GetSoundSettings()
        {
            return _soundSettingsData;
        }

        public WindowsData GetWindowsData()
        {
            return _settingsData.WindowsData;
        }

        public SoundClip GetEffectSoundClip(SfxType sfxType)
        {
            return _soundSettingsData.Effects.SingleOrDefault(x => x.Type == sfxType)?.AudioClip;
        }

        public ProjectileData GetEnemyProjectileData()
        {
            return _settingsData.EnemyProjectileData;
        }

        public bool TryGetTutorialEventData(TutorialEventType tutorialEventType, out TutorialEventData tutorialEventData)
        {
            tutorialEventData = null;

            foreach (var tutorialEvent in _settingsData.TutorialEvents)
            {
                if (tutorialEvent.Type == tutorialEventType)
                {
                    tutorialEventData = tutorialEvent;
                    return true;
                }
            }

            return false;
        }
    }
}