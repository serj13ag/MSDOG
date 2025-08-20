using System;
using System.Collections.Generic;
using Core.Models.Data;
using Gameplay.Providers;

namespace Gameplay.Services
{
    public class DetailService : IDetailService
    {
        private readonly IPlayerProvider _playerProvider;

        private readonly Dictionary<Guid, Detail> _activeDetails = new Dictionary<Guid, Detail>();
        private readonly Dictionary<Guid, Detail> _inactiveDetails = new Dictionary<Guid, Detail>();

        public IReadOnlyDictionary<Guid, Detail> ActiveDetails => _activeDetails;

        public event EventHandler<DetailCreatedEventArgs> OnActiveDetailCreated;
        public event EventHandler<DetailCreatedEventArgs> OnInactiveDetailCreated;

        public DetailService(IPlayerProvider playerProvider)
        {
            _playerProvider = playerProvider;
        }

        public void CreateActiveDetail(AbilityData abilityData)
        {
            var detail = new Detail(abilityData);
            ActivateDetail(detail);

            OnActiveDetailCreated?.Invoke(this, new DetailCreatedEventArgs(detail));
        }

        public void CreateInactiveDetail(AbilityData abilityData)
        {
            var detail = new Detail(abilityData);
            _inactiveDetails.Add(detail.Id, detail);

            OnInactiveDetailCreated?.Invoke(this, new DetailCreatedEventArgs(detail));
        }

        public void ActivateDetail(Guid detailId)
        {
            if (_activeDetails.ContainsKey(detailId))
            {
                return;
            }

            var detailToActivate = _inactiveDetails[detailId];
            ActivateDetail(detailToActivate);
            _inactiveDetails.Remove(detailToActivate.Id);
        }

        public void DeactivateDetail(Guid detailId)
        {
            if (!_activeDetails.TryGetValue(detailId, out var detailToDeactivate))
            {
                return;
            }

            _playerProvider.Player.RemoveAbility(detailId);
            _inactiveDetails.Add(detailToDeactivate.Id, detailToDeactivate);
            _activeDetails.Remove(detailToDeactivate.Id);
        }

        private void ActivateDetail(Detail detail)
        {
            _playerProvider.Player.AddAbility(detail.Id, detail.AbilityData);
            _activeDetails.Add(detail.Id, detail);
        }
    }
}