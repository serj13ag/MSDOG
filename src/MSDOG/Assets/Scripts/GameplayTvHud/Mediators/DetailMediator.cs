using System;
using System.Collections.Generic;
using Core.Services;
using Gameplay;
using Gameplay.Services;
using UnityEngine;

namespace GameplayTvHud.Mediators
{
    public class DetailMediator : IDetailMediator, IDisposable
    {
        private readonly IDetailService _detailService;
        private readonly IDataService _dataService;
        private readonly ILevelFlowService _levelFlowService;

        public event EventHandler<DetailCreatedEventArgs> OnActiveDetailCreated;
        public event EventHandler<DetailCreatedEventArgs> OnInactiveDetailCreated;

        public DetailMediator(IDetailService detailService, IDataService dataService, ILevelFlowService levelFlowService)
        {
            _detailService = detailService;
            _dataService = dataService;
            _levelFlowService = levelFlowService;

            _detailService.OnActiveDetailCreated += DetailServiceOnActiveDetailCreated;
            _detailService.OnInactiveDetailCreated += DetailServiceOnInactiveDetailCreated;
        }

        public IEnumerable<Detail> GetActiveDetails()
        {
            return _detailService.ActiveDetails.Values;
        }

        public void CraftDetail()
        {
            var abilityData = _dataService.GetRandomCraftAbilityData(_levelFlowService.CurrentLevelIndex);
            _detailService.CreateInactiveDetail(abilityData);
        }

        public void ActivateDetail(Detail detail)
        {
            _detailService.ActivateDetail(detail.Id);
        }

        public void DeactivateDetail(Detail detail)
        {
            _detailService.DeactivateDetail(detail.Id);
        }

        public bool HasUpgrade(Detail detail)
        {
            return _detailService.TryGetUpgrade(detail, out _);
        }

        public void UpgradeDetail(Detail detail)
        {
            if (_detailService.TryGetUpgrade(detail, out var upgradedAbilityData))
            {
                _detailService.CreateInactiveDetail(upgradedAbilityData);
            }
            else
            {
                Debug.LogError($"Could not find upgrade for detail with ability data - {detail.AbilityData.name}");
            }
        }

        public void DestructDetail(Detail detail)
        {
            _detailService.DestructDetail(detail.Id);
        }

        public bool CanAddActiveDetail()
        {
            return _detailService.CanAddActiveDetail();
        }

        public bool CanAddInactiveDetail()
        {
            return _detailService.CanAddInactiveDetail();
        }

        private void DetailServiceOnActiveDetailCreated(object sender, DetailCreatedEventArgs e)
        {
            OnActiveDetailCreated?.Invoke(this, e);
        }

        private void DetailServiceOnInactiveDetailCreated(object sender, DetailCreatedEventArgs e)
        {
            OnInactiveDetailCreated?.Invoke(this, e);
        }

        public void Dispose()
        {
            _detailService.OnActiveDetailCreated -= DetailServiceOnActiveDetailCreated;
            _detailService.OnInactiveDetailCreated -= DetailServiceOnInactiveDetailCreated;
        }
    }
}