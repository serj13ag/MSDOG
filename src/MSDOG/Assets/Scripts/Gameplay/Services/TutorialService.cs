using System;
using System.Collections.Generic;
using System.Linq;
using Core.Models.Data;
using Core.Models.SaveData;
using Core.Services;
using Gameplay.Interfaces;

namespace Gameplay.Services
{
    public class TutorialService : ITutorialService, IDisposable
    {
        private const string TutorialSaveDataKey = "TutorialSaveData";

        private readonly IGameplayWindowService _gameplayWindowService;
        private readonly IDataService _dataService;
        private readonly ISaveLoadService _saveLoadService;

        private readonly List<TutorialEventType> _shownTutorialEvents;
        private IEntityWithHealth _entityWithHealth;

        public TutorialService(IGameplayWindowService gameplayWindowService, IDataService dataService, ISaveLoadService saveLoadService)
        {
            _gameplayWindowService = gameplayWindowService;
            _dataService = dataService;
            _saveLoadService = saveLoadService;

            var tutorialSaveData = saveLoadService.Load<TutorialSaveData>(TutorialSaveDataKey);
            _shownTutorialEvents = tutorialSaveData.ShownTutorialEvents;
        }

        public void StartTrackTarget(IEntityWithHealth entityWithHealth)
        {
            _entityWithHealth = entityWithHealth;

            entityWithHealth.OnHealthChanged += OnPlayerHealthChanged;
        }

        public void OnCanCraft()
        {
            TryShowTutorialWindow(TutorialEventType.Craft);
        }

        public void OnHasDetailsWithSimilarAbilities()
        {
            TryShowTutorialWindow(TutorialEventType.Fusion);
        }

        public void OnReloadNeeded()
        {
            TryShowTutorialWindow(TutorialEventType.ReloadTwister);
        }

        public void OnFuseActionDisconnected()
        {
            TryShowTutorialWindow(TutorialEventType.Nitro);
            TryShowTutorialWindow(TutorialEventType.MovementSwitch);
        }

        public void OnLevelActivated()
        {
            TryShowTutorialWindow(TutorialEventType.MovementWASD);
        }

        private void OnPlayerHealthChanged()
        {
            if (_entityWithHealth.CurrentHealth < _entityWithHealth.MaxHealth)
            {
                TryShowTutorialWindow(TutorialEventType.Destruction);
            }
        }

        private void TryShowTutorialWindow(TutorialEventType tutorialEventType)
        {
            if (WasShown(tutorialEventType))
            {
                return;
            }

            if (_dataService.TryGetTutorialEventData(tutorialEventType, out var tutorialEventData))
            {
                _gameplayWindowService.ShowTutorialWindow(tutorialEventData);

                _shownTutorialEvents.Add(tutorialEventType);

                Save();
            }
        }

        private bool WasShown(TutorialEventType type)
        {
            return _shownTutorialEvents.Any(x => x == type);
        }

        private void Save()
        {
            var saveData = new TutorialSaveData(_shownTutorialEvents);
            _saveLoadService.Save(saveData, TutorialSaveDataKey);
        }

        public void Dispose()
        {
            _entityWithHealth.OnHealthChanged -= OnPlayerHealthChanged;
        }
    }
}