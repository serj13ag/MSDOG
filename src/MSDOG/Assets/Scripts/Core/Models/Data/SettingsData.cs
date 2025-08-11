using System.Collections.Generic;
using Gameplay;
using Gameplay.UI;
using UnityEngine;

namespace Core.Models.Data
{
    [CreateAssetMenu(fileName = "SettingsData", menuName = "Data/SettingsData")]
    public class SettingsData : ScriptableObject
    {
        public Player PlayerPrefab;
        public ExperiencePiece ExperiencePiecePrefab;
        public int HealPerDestructedDetail;
        public int ExperiencePerDestructedDetail;
        public bool ShowDebugHitboxes;
        public int[] ExperienceProgression;
        public List<TutorialEventData> TutorialEvents;
        public ProjectileData EnemyProjectileData;
        public WindowsData WindowsData;
        public DamageTextView DamageTextViewPrefab;
    }
}