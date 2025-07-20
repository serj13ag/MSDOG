using System;
using System.Collections.Generic;
using Core.Abilities;

namespace Data
{
    [Serializable]
    public class AbilityUpgradeEntryData
    {
        public AbilityType AbilityType;
        public List<AbilityData> AbilityUpgrades;
    }
}