using UnityEngine;

namespace Services
{
    public class ProgressService
    {
        private const string LastPassedLevelKey = "LastPassedLevel";

        private int _lastPassedLevel;

        public int LastPassedLevel => Application.isEditor ? 50 : _lastPassedLevel;

        public ProgressService()
        {
            _lastPassedLevel = PlayerPrefs.GetInt(LastPassedLevelKey, -1);
        }

        public void SetLastPassedLevel(int level)
        {
            _lastPassedLevel = level;
            PlayerPrefs.SetInt(LastPassedLevelKey, level);
        }
    }
}