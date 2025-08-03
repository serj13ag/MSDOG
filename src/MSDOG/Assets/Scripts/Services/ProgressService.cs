using SaveData;

namespace Services
{
    public class ProgressService
    {
        private const string ProgressSaveDataKey = "ProgressSaveData";

        private readonly SaveLoadService _saveLoadService;

        public int LastPassedLevel { get; private set; }
        public bool EasyModeEnabled { get; private set; }

        public ProgressService(SaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;

            var saveData = saveLoadService.Load<ProgressSaveData>(ProgressSaveDataKey);
            LastPassedLevel = saveData.LastPassedLevel;
            EasyModeEnabled = saveData.EasyModeEnabled;
        }

        public void UnlockAllLevels()
        {
            SetLastPassedLevel(50);
        }

        public void SetLastPassedLevel(int level)
        {
            LastPassedLevel = level;
            Save();
        }

        public void SetEasyMode(bool easyModeEnabled)
        {
            EasyModeEnabled = easyModeEnabled;
            Save();
        }

        private void Save()
        {
            var saveData = new ProgressSaveData(LastPassedLevel, EasyModeEnabled);
            _saveLoadService.Save(saveData, ProgressSaveDataKey);
        }
    }
}