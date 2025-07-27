using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Data/LevelData")]
    public class LevelData : ScriptableObject
    {
        public int LevelIndex;
        public string LevelName;
        public List<WaveData> Waves;
        public DialogueData StartDialogue;
        public DialogueData EndDialogue;
    }
}