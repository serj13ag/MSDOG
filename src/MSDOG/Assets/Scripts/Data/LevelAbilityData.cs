using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "LevelAbilityData", menuName = "Data/LevelAbilityData")]
    public class LevelAbilityData : ScriptableObject
    {
        public AbilityData StartAbility;
        public List<AbilityData> AbilitiesAvailableToCraft;
    }
}