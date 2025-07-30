using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "SettingsData", menuName = "Data/SettingsData")]
    public class SettingsData : ScriptableObject
    {
        public int HealPerDestructedDetail;
        public int ExperiencePerDestructedDetail;
        public bool ShowDebugHitboxes;
        public int[] ExperienceProgression;
        public List<TutorialEventData> TutorialEvents;
    }
}