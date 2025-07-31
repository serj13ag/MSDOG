using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Data;
using Newtonsoft.Json;
using UnityEngine;

namespace Services.Gameplay
{
    public class TutorialService : IDisposable
    {
        private const string PlayerPrefsKey = "TutorialSaveData";

        private readonly WindowService _windowService;
        private readonly DataService _dataService;

        private readonly List<TutorialEventType> _shownTutorialEvents;
        private Player _player;

        public TutorialService(WindowService windowService, DataService dataService)
        {
            _windowService = windowService;
            _dataService = dataService;

            _shownTutorialEvents = LoadShownTutorialEvents();
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
                _windowService.ShowTutorialWindow(tutorialEventData);

                _shownTutorialEvents.Add(tutorialEventType);
                Save();
            }
        }

        private bool WasShown(TutorialEventType type)
        {
            return _shownTutorialEvents.Any(x => x == type);
        }

        private List<TutorialEventType> LoadShownTutorialEvents()
        {
            var json = PlayerPrefs.GetString(PlayerPrefsKey);
            return string.IsNullOrEmpty(json)
                ? new List<TutorialEventType>()
                : JsonConvert.DeserializeObject<List<TutorialEventType>>(json);
        }

        private void Save()
        {
            var json = JsonConvert.SerializeObject(_shownTutorialEvents);
            PlayerPrefs.SetString(PlayerPrefsKey, json);
            PlayerPrefs.Save();
        }

        public void Dispose()
        {
            _player.OnHealthChanged -= OnPlayerHealthChanged;
        }
    }
}