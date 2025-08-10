using System.Collections.Generic;
using Gameplay;
using UnityEngine;

namespace Core.Models.Data
{
    [CreateAssetMenu(fileName = "SettingsData", menuName = "Data/SettingsData")]
    public class SettingsData : ScriptableObject
    {
        public Player PlayerPrefab;
        public int HealPerDestructedDetail;
        public int ExperiencePerDestructedDetail;
        public bool ShowDebugHitboxes;
        public int[] ExperienceProgression;
        public List<TutorialEventData> TutorialEvents;
        public ProjectileData EnemyProjectileData;
    }
}