using System.Collections.Generic;
using Core.Models.Data;
using Core.Sounds;
using Gameplay.Abilities;

namespace Core.Services
{
    public interface IDataService
    {
        int GetNumberOfLevels();
        LevelData GetLevelData(int levelIndex);

        List<AbilityData> GetStartAbilitiesData(int levelIndex);
        List<AbilityData> GetAbilitiesAvailableToCraft(int levelIndex);
        AbilityData GetRandomCraftAbilityData(int levelIndex);
        IEnumerable<AbilityData> GetAbilitiesData();
        bool TryGetAbilityUpgradeData(AbilityType abilityType, int level, out AbilityData upgradedAbilityData);

        SettingsData GetSettings();
        SoundSettingsData GetSoundSettings();
        WindowsData GetWindowsData();

        SoundClip GetEffectSoundClip(SfxType sfxType);

        ProjectileData GetEnemyProjectileData();

        bool TryGetTutorialEventData(TutorialEventType tutorialEventType, out TutorialEventData tutorialEventData);
    }
}