using System;
using Core.Services;

namespace Gameplay.Services
{
    public class DialogueService : IDialogueService
    {
        private readonly IDataService _dataService;
        private readonly IGameplayWindowService _gameplayWindowService;

        public DialogueService(IDataService dataService, IGameplayWindowService gameplayWindowService)
        {
            _dataService = dataService;
            _gameplayWindowService = gameplayWindowService;
        }

        public bool TryShowStartLevelDialogue(int levelIndex, Action onDialogueCompleted)
        {
            var levelData = _dataService.GetLevelData(levelIndex);
            if (!levelData.StartDialogue)
            {
                return false;
            }

            _gameplayWindowService.ShowDialogueWindow(levelData.StartDialogue, onDialogueCompleted);
            return true;
        }

        public bool TryShowEndLevelDialogue(int levelIndex, Action onDialogueCompleted)
        {
            var levelData = _dataService.GetLevelData(levelIndex);
            if (!levelData.EndDialogue)
            {
                return false;
            }

            _gameplayWindowService.ShowDialogueWindow(levelData.EndDialogue, onDialogueCompleted);
            return true;
        }
    }
}