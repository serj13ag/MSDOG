using System;
using System.Collections.Generic;
using Gameplay;

namespace GameplayTvHud.Mediators
{
    public interface IDetailMediator
    {
        event EventHandler<DetailCreatedEventArgs> OnActiveDetailCreated;
        event EventHandler<DetailCreatedEventArgs> OnInactiveDetailCreated;

        IEnumerable<Detail> GetActiveDetails();

        void CraftDetail();

        void AddActiveDetail(Detail detail);
        void RemoveActiveDetail(Detail detail);
        void AddInactiveDetail(Detail detail);
        void RemoveInactiveDetail(Detail detail);
        void DetailDestructed();

        bool HasUpgrade(Detail detail);
        void UpgradeDetail(Detail detail);

        bool CanAddActiveDetail();
        bool CanAddInactiveDetail();
    }
}