using System;
using System.Collections.Generic;
using Core.Models.Data;

namespace Gameplay.Services
{
    public interface IDetailService
    {
        IReadOnlyDictionary<Guid, Detail> ActiveDetails { get; }

        void CreateActiveDetail(AbilityData abilityData);
        void CreateInactiveDetail(AbilityData abilityData);

        bool CanAddActiveDetail();
        bool CanAddInactiveDetail();

        void AddActiveDetail(Detail detail);
        void RemoveActiveDetail(Guid detailId);
        void AddInactiveDetail(Detail detail);
        void RemoveInactiveDetail(Detail detail);

        bool TryGetUpgrade(Detail detail, out AbilityData upgradedAbilityData);

        event EventHandler<DetailCreatedEventArgs> OnActiveDetailCreated;
        event EventHandler<DetailCreatedEventArgs> OnInactiveDetailCreated;
    }
}