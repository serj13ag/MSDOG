using System;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "DialogueData", menuName = "Data/DialogueData")]
    public class DialogueData : ScriptableObject
    {
        public DialogueStage[] DialogueStages;
    }

    [Serializable]
    public class DialogueStage
    {
        public Sprite Avatar;
        public string Name;
        public string Speech;
    }
}