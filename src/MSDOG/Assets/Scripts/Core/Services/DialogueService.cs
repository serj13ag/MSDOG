using System;
using Core.Controllers;

namespace Core.Services
{
    public class DialogueService
    {
        private readonly DataService _dataService;
        private readonly WindowController _windowController;

        public DialogueService(DataService dataService, WindowController windowController)
        {
            _dataService = dataService;
            _windowController = windowController;
        }

        public bool TryShowStartLevelDialogue(int levelIndex, Action onDialogueCompleted)
        {
            var levelData = _dataService.GetLevelData(levelIndex);
            if (!levelData.StartDialogue)
            {
                return false;
            }

            _windowController.ShowDialogueWindow(levelData.StartDialogue, onDialogueCompleted);
            return true;
        }

        public bool TryShowEndLevelDialogue(int levelIndex, Action onDialogueCompleted)
        {
            var levelData = _dataService.GetLevelData(levelIndex);
            if (!levelData.EndDialogue)
            {
                return false;
            }

            _windowController.ShowDialogueWindow(levelData.EndDialogue, onDialogueCompleted);
            return true;
        }
    }
}