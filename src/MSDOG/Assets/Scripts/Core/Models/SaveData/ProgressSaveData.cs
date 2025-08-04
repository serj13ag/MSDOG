using System;

namespace Core.Models.SaveData
{
    [Serializable]
    public class ProgressSaveData
    {
        public int LastPassedLevel = -1;
        public bool EasyModeEnabled = false;

        public ProgressSaveData()
        {
        }

        public ProgressSaveData(int lastPassedLevel, bool easyModeEnabled)
        {
            LastPassedLevel = lastPassedLevel;
            EasyModeEnabled = easyModeEnabled;
        }
    }
}