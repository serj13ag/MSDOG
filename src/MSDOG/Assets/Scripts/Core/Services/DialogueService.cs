using System;
using Core.Controllers;

namespace Core.Services
{
    public class DialogueService
    {
        private readonly DataService _dataService;
        private readonly WindowService _windowService;

        public DialogueService(DataService dataService, WindowService windowService)
        {
            _dataService = dataService;
            _windowService = windowService;
        }

        public bool TryShowStartLevelDialogue(int levelIndex, Action onDialogueCompleted)
        {
            var levelData = _dataService.GetLevelData(levelIndex);
            if (!levelData.StartDialogue)
            {
                return false;
            }

            _windowService.ShowDialogueWindow(levelData.StartDialogue, onDialogueCompleted);
            return true;
        }

        public bool TryShowEndLevelDialogue(int levelIndex, Action onDialogueCompleted)
        {
            var levelData = _dataService.GetLevelData(levelIndex);
            if (!levelData.EndDialogue)
            {
                return false;
            }

            _windowService.ShowDialogueWindow(levelData.EndDialogue, onDialogueCompleted);
            return true;
        }
    }
}