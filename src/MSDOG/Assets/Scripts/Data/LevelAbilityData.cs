using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "LevelAbilityData", menuName = "Data/LevelAbilityData")]
    public class LevelAbilityData : ScriptableObject
    {
        public List<AbilityData> StartAbilities;
        public List<AbilityData> AbilitiesAvailableToCraft;
    }
}