using System.Collections.Generic;
using Gameplay;
using Gameplay.UI;
using GameplayTvHud.DetailsZone;
using UnityEngine;

namespace Core.Models.Data
{
    [CreateAssetMenu(fileName = "SettingsData", menuName = "Data/SettingsData")]
    public class SettingsData : ScriptableObject
    {
        public Player PlayerPrefab;
        public ExperiencePiece ExperiencePiecePrefab;

        public int HealPerDestructedDetail = 10;
        public int ExperiencePerDestructedDetail = 5;
        public int MaxNumberOfActiveDetails = 6;
        public int MaxNumberOfInactiveDetails = 18;

        public bool ShowDebugHitboxes;
        public int[] ExperienceProgression;
        public List<TutorialEventData> TutorialEvents;

        public ProjectileData EnemyProjectileData;
        public WindowsData WindowsData;

        public DamageTextView DamageTextViewPrefab;
        public DetailView DetailViewPrefab;
        public DetailGhostView DetailGhostViewPrefab;
    }
}