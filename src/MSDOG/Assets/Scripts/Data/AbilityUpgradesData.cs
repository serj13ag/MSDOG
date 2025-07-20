using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "AbilityUpgradesData", menuName = "Data/AbilityUpgradesData")]
    public class AbilityUpgradesData : ScriptableObject
    {
        public List<AbilityUpgradeEntryData> AbilityUpgrades;
    }
}