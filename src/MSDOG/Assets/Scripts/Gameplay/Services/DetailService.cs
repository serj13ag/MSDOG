using System;
using System.Collections.Generic;
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
        private readonly int _maxNumberOfInactiveDetails;

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
            _maxNumberOfInactiveDetails = settings.MaxNumberOfInactiveDetails;
        }

        public void CreateActiveDetail(AbilityData abilityData)
        {
            if (!CanAddActiveDetail())
            {
                Debug.LogError("Max number of active details exceeded!");
                return;
            }

            var detail = CreateDetail(abilityData);
            ActivateDetail(detail);

            OnActiveDetailCreated?.Invoke(this, new DetailCreatedEventArgs(detail));
        }

        public void CreateInactiveDetail(AbilityData abilityData)
        {
            if (!CanAddInactiveDetail())
            {
                Debug.LogError("Max number of inactive details exceeded!");
                return;
            }

            var detail = CreateDetail(abilityData);
            _inactiveDetails.Add(detail.Id, detail);

            if (HasDetailsWithSimilarAbilities())
            {
                _tutorialService.OnHasDetailsWithSimilarAbilities();
            }

            OnInactiveDetailCreated?.Invoke(this, new DetailCreatedEventArgs(detail));
        }

        public bool CanAddActiveDetail()
        {
            return _activeDetails.Count < _maxNumberOfActiveDetails;
        }

        public bool CanAddInactiveDetail()
        {
            return _inactiveDetails.Count < _maxNumberOfInactiveDetails;
        }

        public void AddActiveDetail(Detail detail)
        {
            if (!CanAddActiveDetail())
            {
                Debug.LogError("Max number of active details exceeded!");
                return;
            }

            if (_activeDetails.ContainsKey(detail.Id))
            {
                Debug.LogError("That detail already activated!");
                return;
            }

            ActivateDetail(detail);
        }

        public void RemoveActiveDetail(Guid detailId)
        {
            if (!_activeDetails.TryGetValue(detailId, out var detailToDeactivate))
            {
                Debug.LogError("Detail not found!");
                return;
            }

            _playerProvider.Player.RemoveAbility(detailId);
            _activeDetails.Remove(detailToDeactivate.Id);
        }

        public void AddInactiveDetail(Detail detail)
        {
            if (!CanAddInactiveDetail())
            {
                Debug.LogError("Max number of inactive details exceeded!");
                return;
            }

            _inactiveDetails.Add(detail.Id, detail);
        }

        public void RemoveInactiveDetail(Detail detail)
        {
            _inactiveDetails.Remove(detail.Id);
        }

        public bool TryGetUpgrade(Detail detail, out AbilityData upgradedAbilityData)
        {
            var abilityData = detail.AbilityData;
            return _dataService.TryGetAbilityUpgradeData(abilityData.AbilityType, abilityData.Level, out upgradedAbilityData);
        }

        private static Detail CreateDetail(AbilityData abilityData)
        {
            var detail = new Detail(abilityData);
            return detail;
        }

        private void ActivateDetail(Detail detail)
        {
            _playerProvider.Player.AddAbility(detail.Id, detail.AbilityData);
            _activeDetails.Add(detail.Id, detail);
        }

        private bool HasDetailsWithSimilarAbilities()
        {
            var abilities = new HashSet<AbilityData>();

            foreach (var activeDetail in _activeDetails)
            {
                if (!abilities.Add(activeDetail.Value.AbilityData))
                {
                    return true;
                }
            }

            foreach (var inactiveDetail in _inactiveDetails)
            {
                if (!abilities.Add(inactiveDetail.Value.AbilityData))
                {
                    return true;
                }
            }

            return false;
        }
    }
}