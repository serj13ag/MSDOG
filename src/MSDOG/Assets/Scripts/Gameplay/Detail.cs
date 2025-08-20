using System;
using Core.Models.Data;

namespace Gameplay
{
    public class Detail
    {
        public Guid Id { get; }
        public AbilityData AbilityData { get; }

        public Detail(AbilityData abilityData)
        {
            Id = Guid.NewGuid();
            AbilityData = abilityData;
        }
    }
}