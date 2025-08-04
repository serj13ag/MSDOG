using System;
using System.Collections.Generic;
using Gameplay.Abilities;

namespace Core.Models.Data
{
    [Serializable]
    public class AbilityUpgradeEntryData
    {
        public AbilityType AbilityType;
        public List<AbilityData> AbilityUpgrades;
    }
}