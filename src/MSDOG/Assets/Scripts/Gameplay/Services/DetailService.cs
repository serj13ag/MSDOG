using System;
using System.Collections.Generic;
using System.Linq;
using Core.Models.Data;
using Core.Services;
using Gameplay.Providers;
using UnityEngine;

namespace Gameplay.Services
{
    public class DetailService : IDetailService
    {
        private readonly IPlayerProvider _playerProvider;
        private readonly ITutorialService _tutorialService;
        private readonly IDataService _dataService;

        private readonly int _maxNumberOfActiveDetails;

        private readonly Dictionary<Guid, Detail> _activeDetails = new Dictionary<Guid, Detail>();
        private readonly Dictionary<Guid, Detail> _inactiveDetails = new Dictionary<Guid, Detail>();

        public IReadOnlyDictionary<Guid, Detail> ActiveDetails => _activeDetails;

        public event EventHandler<DetailCreatedEventArgs> OnActiveDetailCreated;
        public event EventHandler<DetailCreatedEventArgs> OnInactiveDetailCreated;

        public DetailService(IPlayerProvider playerProvider, ITutorialService tutorialService, IDataService dataService)
        {
            _tutorialService = tutorialService;
            _dataService = dataService;
            _playerProvider = playerProvider;

            var settings = dataService.GetSettingsData();
            _maxNumberOfActiveDetails = settings.MaxNumberOfActiveDetails;
        }

        public void CreateActiveDetail(AbilityData abilityData)
        {
            if (!CanAddActiveDetail())
            {
                Debug.LogError("Max number of active details exceeded!");
                return;
            }

            var detail = new Detail(abilityData);
            ActivateDetail(detail);

            OnActiveDetailCreated?.Invoke(this, new DetailCreatedEventArgs(detail));
        }

        public void CreateInactiveDetail(AbilityData abilityData)
        {
            var detail = new Detail(abilityData);
            _inactiveDetails.Add(detail.Id, detail);

            if (HasSameDetails())
            {
                _tutorialService.OnHasTwoSameDetails();
            }

            OnInactiveDetailCreated?.Invoke(this, new DetailCreatedEventArgs(detail));
        }

        public bool CanAddActiveDetail()
        {
            return _activeDetails.Count < _maxNumberOfActiveDetails;
        }

        public void ActivateDetail(Guid detailId)
        {
            if (!CanAddActiveDetail())
            {
                Debug.LogError("Max number of active details exceeded!");
                return;
            }

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

        public bool TryGetUpgrade(Detail detail, out AbilityData upgradedAbilityData)
        {
            var abilityData = detail.AbilityData;
            return _dataService.TryGetAbilityUpgradeData(abilityData.AbilityType, abilityData.Level, out upgradedAbilityData);
        }

        private void ActivateDetail(Detail detail)
        {
            _playerProvider.Player.AddAbility(detail.Id, detail.AbilityData);
            _activeDetails.Add(detail.Id, detail);
        }

        private bool HasSameDetails()
        {
            // TODO: refactor add comparer
            var allDetails = new List<Detail>(_activeDetails.Values);
            allDetails.AddRange(_inactiveDetails.Values);
            return allDetails
                .GroupBy(a => new { a.AbilityData.AbilityType, a.AbilityData.Level })
                .Any(g => g.Count() > 1);
        }
    }
}