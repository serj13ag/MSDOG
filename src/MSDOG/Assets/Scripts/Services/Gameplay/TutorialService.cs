using System.Linq;
using Data;
using UnityEngine;

namespace Services.Gameplay
{
    public class TutorialService
    {
        private readonly WindowService _windowService;
        private readonly DataService _dataService;

        private bool _craftShown;

        public TutorialService(WindowService windowService, DataService dataService)
        {
            _windowService = windowService;
            _dataService = dataService;

            _craftShown = PlayerPrefs.GetInt("TutorialCraftShown", 0) == 1;
        }

        public void OnCanCraft()
        {
            if (_craftShown)
            {
                return;
            }

            var tutorialEventData = _dataService.GetSettingsData()
                .TutorialEvents
                .SingleOrDefault(x => x.Type == TutorialEventType.Craft);

            if (tutorialEventData != null)
            {
                _windowService.ShowTutorialWindow(tutorialEventData);

                _craftShown = true;
                PlayerPrefs.SetInt("TutorialCraftShown", 1);
            }
        }
    }
}