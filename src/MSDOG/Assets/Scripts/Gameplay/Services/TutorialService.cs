using System;
using System.Collections.Generic;
using System.Linq;
using Core.Controllers;
using Core.Models.Data;
using Core.Models.SaveData;
using Core.Services;

namespace Gameplay.Services
{
    public class TutorialService : IDisposable
    {
        private const string TutorialSaveDataKey = "TutorialSaveData";

        private readonly WindowController _windowController;
        private readonly IDataService _dataService;
        private readonly ISaveLoadService _saveLoadService;

        private readonly List<TutorialEventType> _shownTutorialEvents;
        private Player _player;

        public TutorialService(WindowController windowController, IDataService dataService, ISaveLoadService saveLoadService)
        {
            _windowController = windowController;
            _dataService = dataService;
            _saveLoadService = saveLoadService;

            var tutorialSaveData = saveLoadService.Load<TutorialSaveData>(TutorialSaveDataKey);
            _shownTutorialEvents = tutorialSaveData.ShownTutorialEvents;
        }

        public void SetPlayer(Player player)
        {
            _player = player;

            player.OnHealthChanged += OnPlayerHealthChanged;
        }

        public void OnCanCraft()
        {
            TryShowTutorialWindow(TutorialEventType.Craft);
        }

        public void OnHasTwoSameDetails()
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
            if (_player.CurrentHealth < _player.MaxHealth)
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

            var tutorialEventData = _dataService.GetSettingsData()
                .TutorialEvents
                .SingleOrDefault(x => x.Type == tutorialEventType);

            if (tutorialEventData != null)
            {
                _windowController.ShowTutorialWindow(tutorialEventData);

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
            _player.OnHealthChanged -= OnPlayerHealthChanged;
        }
    }
}