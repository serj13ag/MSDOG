using System;
using System.Collections.Generic;
using Gameplay;

namespace UI.HUD
{
    public interface IDetailMediator
    {
        event EventHandler<DetailCreatedEventArgs> OnActiveDetailCreated;
        event EventHandler<DetailCreatedEventArgs> OnInactiveDetailCreated;

        IEnumerable<Detail> GetActiveDetails();

        void CraftDetail();

        void ActivateDetail(Detail detail);
        void DeactivateDetail(Detail detail);

        bool HasUpgrade(Detail detail);
        void UpgradeDetail(Detail detail);

        void DetailDestructed();
    }
}