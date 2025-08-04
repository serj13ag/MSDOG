using System;
using System.Collections.Generic;
using Core.Models.Data;

namespace Core.Models.SaveData
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