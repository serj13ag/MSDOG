using System;
using System.Collections.Generic;
using Data;

namespace SaveData
{
    [Serializable]
    public class TutorialSaveData
    {
        public List<TutorialEventType> ShownTutorialEvents = new List<TutorialEventType>();

        public TutorialSaveData()
        {
        }

        public TutorialSaveData(List<TutorialEventType> shownTutorialEvents)
        {
            ShownTutorialEvents = shownTutorialEvents;
        }
    }
}