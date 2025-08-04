using UnityEngine;

namespace Core.Models.Data
{
    [CreateAssetMenu(fileName = "TutorialEventData", menuName = "Data/TutorialEventData")]
    public class TutorialEventData : ScriptableObject
    {
        public TutorialEventType Type;
        public string Title;
        public string HintText;
        public Sprite Image;
    }
}