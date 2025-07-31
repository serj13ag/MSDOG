using UnityEngine;

namespace Services
{
    public class ProgressService
    {
        private const string LastPassedLevelKey = "LastPassedLevel";
        private const string EasyModeEnabledKey = "EasyModeEnabledKey";

        private int _lastPassedLevel;
        private bool _easyModeEnabled;

        public int LastPassedLevel => _lastPassedLevel;
        public bool EasyModeEnabled => _easyModeEnabled;

        public ProgressService()
        {
            _lastPassedLevel = PlayerPrefs.GetInt(LastPassedLevelKey, -1);
            _easyModeEnabled = PlayerPrefs.GetInt(EasyModeEnabledKey, 0) == 1;
        }

        public void SetLastPassedLevel(int level)
        {
            _lastPassedLevel = level;
            PlayerPrefs.SetInt(LastPassedLevelKey, level);
            PlayerPrefs.Save();
        }

        public void UnlockAllLevels()
        {
            SetLastPassedLevel(50);
        }

        public void SetEasyMode(bool easyModeEnabled)
        {
            _easyModeEnabled = easyModeEnabled;
            PlayerPrefs.SetInt(EasyModeEnabledKey, _easyModeEnabled ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}